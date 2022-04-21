
namespace helloAPI.Controllers;

[ApiController,Route("users")]
public class UsersController:ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = null!;
    private readonly SignInManager<IdentityUser> _signInManager = null!;
    private readonly ApplicationDbContext _applicationDbContext = null!;

    private readonly IConfiguration _configuration = null!;
    private readonly IHostEnvironment _hostEnvironment = null!;


    public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext applicationDbContext, IConfiguration configuration, IHostEnvironment hostEnvironment ){
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationDbContext = applicationDbContext;
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet,Authorize]
    public async Task<IActionResult> Get(){
        /* var query = from user in _applicationDbContext.Users
                    join userDetails in _applicationDbContext.UserDetails on user.Id equals userDetails.AspNetUserId
                    select new { 
                        Email = user.Email
                    };
        return Ok(query); */

        var userLists = await _applicationDbContext.Users
                .Join( _applicationDbContext.UserDetails,
                        a => a.Id,
                        b => b.AspNetUserId,
                        (a, b) => new {
                            Id = a.Id,
                            Email = a.Email,
                            Firstname = b.Firstname,
                            Lastname = b.Lastname,
                            Birthdate = b.Birthdate.ToString("yyyy-MM-dd"),
                            ProfileImage =  b.ProfileImage == null ? "" : $"{HttpContext.Request.Scheme }://" + HttpContext.Request.Host.ToUriComponent() + "/uploads/" + b.ProfileImage,
                            Balance = b.Balance
                        }
                     ).ToListAsync();
        return Ok(userLists);
    }

    [HttpPost,Route("register")]
    public  async Task<IActionResult> registerUser( RegisterUser registerUser){
        var user = new IdentityUser { UserName = registerUser.Email, Email = registerUser.Email };
        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if( result.Succeeded){
            await _userManager.AddToRoleAsync(user, "User");


            UserDetails userDetails = new UserDetails(){
                Firstname = registerUser.Firstname,
                Lastname = registerUser.Lastname,
                Balance = 0.0m,
                Birthdate = registerUser.Birthdate,
                AspNetUserId = user.Id,
                
            };


            _applicationDbContext.Add(userDetails);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
        else{
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Registration failed: " + string.Join("\n", result.Errors.Select(e => e.Description).ToList() )  } );
        }
        
        
    }

    [HttpGet("{id}"),Authorize]
    public async Task<IActionResult> Get( string? id ){

        var loggedInUser = await _userManager.FindByIdAsync(id);

        var loggedInUserDetails = await _applicationDbContext.UserDetails.Where(u => u.AspNetUserId == loggedInUser.Id ).FirstOrDefaultAsync();
        return Ok(new{
            Id = loggedInUser.Id,
            Firstname = loggedInUserDetails?.Firstname,
            Lastname = loggedInUserDetails?.Lastname,
            Birthdate = loggedInUserDetails?.Birthdate.ToString("yyyy-MM-dd"),
            ProfileImage =  loggedInUserDetails?.ProfileImage == null ? "" : $"{HttpContext.Request.Scheme }://" + HttpContext.Request.Host.ToUriComponent() + "/uploads/" + loggedInUserDetails?.ProfileImage,
            Balance = loggedInUserDetails?.Balance
        });

    }

    [HttpPost,Route("login")]
    public async Task<IActionResult> loginUser( LoginUser loginUser){

        if( ModelState.IsValid ){
            var result = await _signInManager.PasswordSignInAsync( loginUser.email, loginUser.password, isPersistent: false, lockoutOnFailure: true);

            if( result.Succeeded ){

                var loggedInUser = await _userManager.FindByEmailAsync(loginUser.email);

                var userRoles = await _userManager.GetRolesAsync( loggedInUser );

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loggedInUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                var existingRefreshtoken = await _applicationDbContext.UserRefreshtokens.Where( r => r.AspNetUserId == loggedInUser.Id  ).FirstOrDefaultAsync();

                if( existingRefreshtoken == null ){

                    UserRefreshtokens userRefreshtokens = new UserRefreshtokens(){
                        Refreshtoken = refreshToken,
                        AspNetUserId = loggedInUser.Id,
                        Expiry = DateTime.Now.AddDays(1)
                    };
                    _applicationDbContext.Add( userRefreshtokens );
                }
                else{
                    existingRefreshtoken.Refreshtoken = refreshToken;
                    existingRefreshtoken.Expiry = DateTime.Now.AddDays(1);
                    _applicationDbContext.Entry( existingRefreshtoken ).State = EntityState.Modified;
                }
                await _applicationDbContext.SaveChangesAsync();

                return Ok(new
                {
                    User = new { Id = loggedInUser.Id },
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
        }

        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Login Failed"  } );
    }

/// <param name="id" description="id of the user to be updated"></param>
/// <param name="profilePicture" description="upload via formdata"></param>
/// <remarks>
/// upload via form data field named "profilePicture" (multipart/form-data)
/// </remarks>
/// <returns></returns>
    [HttpPost,Authorize,Route("{id}/uploadProfilePicture")]
        public async Task<IActionResult> updateProfilePicture(string id, IFormFile profilePicture){

            var updatedUser = await _userManager.FindByIdAsync(id);

            if( updatedUser == null ){
                return Unauthorized();
            }

            string uploadPath = Path.Combine(_hostEnvironment.ContentRootPath , "wwwroot/uploads" );
            if( !Directory.Exists(uploadPath)){
                Directory.CreateDirectory(uploadPath);
            }
            var newFilename = Path.GetRandomFileName() + Path.GetExtension( profilePicture.FileName );


            using( FileStream stream = new FileStream(Path.Combine(uploadPath, newFilename ), FileMode.Create ) ){
                await profilePicture.CopyToAsync(stream);
            }

        /*    var username = HttpContext.User.Identity?.Name; 

            var loggedInUser = await _userManager.FindByNameAsync( username ); */

            

            /*UserDetails userDetails = new UserDetails(){
                ProfileImage = newFilename,
                AspNetUserId = loggedInUser.Id,
            };*/

            var updatedUserDetails = await _applicationDbContext.UserDetails.Where(u => u.AspNetUserId == updatedUser.Id ).FirstOrDefaultAsync();
            
            updatedUserDetails!.ProfileImage = newFilename;

            _applicationDbContext.Update(updatedUserDetails);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(new { message = "Sucessfully updated profile picture" });

        }


        [HttpPost,Route("refreshToken")]
        public async Task<IActionResult> RefreshToken( string accessToken, string refreshToken)
        {
            if (accessToken == null || refreshToken == null )
            {
                return BadRequest("Invalid client request");
            }

            //string? accessToken = accessToken;
            //string? refreshToken = refreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal!.Identity!.Name!;


            var user = await _userManager.FindByNameAsync(username);

            
            var userRefreshToken = await _applicationDbContext.UserRefreshtokens.Where(r => r.AspNetUserId == user.Id && r.Refreshtoken == refreshToken ).FirstOrDefaultAsync();

            

            if (user == null || userRefreshToken == null || userRefreshToken.Expiry <= DateTime.Now )
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            //user.RefreshToken = newRefreshToken;
            //await _userManager.UpdateAsync(user);

            userRefreshToken.Refreshtoken = newRefreshToken;
            userRefreshToken.Expiry = DateTime.Now.AddDays(1);
            _applicationDbContext.Entry(userRefreshToken).State = EntityState.Modified;
            _applicationDbContext.SaveChanges();

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

    [HttpPut("{id}"),Authorize]
    public async Task<IActionResult> Update( string id, UpdateUser updateUser){
        if( ModelState.IsValid ){
            
            var updatedUser = await _userManager.FindByIdAsync(id);

            var updatedUserDetails = await _applicationDbContext.UserDetails.Where( u => u.AspNetUserId == updatedUser.Id ).FirstOrDefaultAsync();

            updatedUserDetails!.Firstname = updateUser.Firstname!;
            updatedUserDetails.Lastname = updateUser.Lastname!;
            updatedUserDetails.Birthdate = updateUser.Birthdate!;

            _applicationDbContext.Entry(updatedUserDetails).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();

            /* return Ok(new{
                Firstname = updatedUserDetails.Firstname,
                Lastname = updatedUserDetails.Lastname,
                Birthdate = updatedUserDetails.Birthdate
            }); */

            return NoContent();

        }
        else{
            return BadRequest();
        }
    }

    [HttpPost,Route("{id}/updateBalance"),Authorize]
    public async Task<IActionResult> SetBalance(string id,Decimal balanceAmount){
        //var loggedInUser = await _userManager.FindByNameAsync( HttpContext.User.Identity?.Name );
        var updatedUser = await _userManager.FindByIdAsync(id);

        var updatedUserDetails = await _applicationDbContext.UserDetails.Where( u => u.AspNetUserId == updatedUser.Id ).FirstOrDefaultAsync();

        updatedUserDetails!.Balance += balanceAmount;

        _applicationDbContext.Entry(updatedUserDetails).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();

        return Ok( new {
            message = "Successfully updated the balance",
            User = new {
                Id = updatedUser.Id,
                balance = updatedUserDetails.Balance
            }
            }
        );
    }

    [HttpDelete("{id}"),Authorize(Roles = "Super Administrator,Administrator")]
    public async Task<IActionResult> Delete( string id ){
        IdentityUser userToBeDeleted = await _userManager.FindByIdAsync(id);

        
        var userDetails = await _applicationDbContext.UserDetails.Where( u => u.AspNetUserId == userToBeDeleted.Id ).FirstOrDefaultAsync();
        var userRefreshTokens = await _applicationDbContext.UserRefreshtokens.Where( u => u.AspNetUserId == userToBeDeleted.Id ).FirstOrDefaultAsync();
        //if( result.Succeeded ){
            if( userDetails != null )
                _applicationDbContext.UserDetails.Remove( userDetails );
            if( userRefreshTokens != null )
                _applicationDbContext.UserRefreshtokens.Remove( userRefreshTokens );
        var result  = await _applicationDbContext.SaveChangesAsync();

        if( result > 0 ){
             await _userManager.DeleteAsync( userToBeDeleted );

            return Ok(new { message = "Successfully deleted the user" });
        }
        else{
            return StatusCode(StatusCodes.Status500InternalServerError, new {message = "Error deleting the user"} );
        }

    }




        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            _ = int.TryParse(_configuration["JWT:DurationInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
}