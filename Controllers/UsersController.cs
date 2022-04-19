
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
                            Email = a.Email,
                            Firstname = b.Firstname,
                            Lastname = b.Lastname,
                            Birthdate = b.Birthdate.ToString("yyyy-MM-dd"),
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

/// <summary>
/// get details of the logged in user OR the details of the owner of the email parameter
/// </summary>
/// <param name="email" description="optional"></param>
    [HttpGet,Route("detail"),Authorize]
    public async Task<IActionResult> Get( string? email = null ){
        var username = email is not null ? email : HttpContext.User.Identity?.Name; 

        var loggedInUser = await _userManager.FindByNameAsync( username );

        var loggedInUserDetails = _applicationDbContext.UserDetails.Where(u => u.AspNetUserId == loggedInUser.Id ).FirstOrDefault();
        return Ok(new{
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
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
        }

        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Login Failed"  } );
    }
/// <summary>
/// Updates profile picture for the requesting user with access token
/// </summary>
/// <param name="profilePicture" description="upload via formdata"></param>
/// <remarks>
/// upload via form data field named "profilePicture" (multipart/form-data)
/// </remarks>
/// <returns></returns>
    [HttpPost,Authorize,Route("uploadProfilePicture")]
        public async Task<IActionResult> updateProfilePicture(IFormFile profilePicture){

            string uploadPath = Path.Combine(_hostEnvironment.ContentRootPath , "wwwroot/uploads" );
            if( !Directory.Exists(uploadPath)){
                Directory.CreateDirectory(uploadPath);
            }
            var newFilename = Path.GetRandomFileName() + Path.GetExtension( profilePicture.FileName );


            using( FileStream stream = new FileStream(Path.Combine(uploadPath, newFilename ), FileMode.Create ) ){
                await profilePicture.CopyToAsync(stream);
            }

            var username = HttpContext.User.Identity?.Name; 

            var loggedInUser = await _userManager.FindByNameAsync( username );

            UserDetails updatedUserDetails =  _applicationDbContext.UserDetails.Where(u => u.AspNetUserId == loggedInUser.Id ).FirstOrDefault()!;
            
            updatedUserDetails.ProfileImage = newFilename;

            /*UserDetails userDetails = new UserDetails(){
                ProfileImage = newFilename,
                AspNetUserId = loggedInUser.Id,
            };*/

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

    [HttpPut,Route("update"),Authorize]
    public async Task<IActionResult> Update( UpdateUser updateUser){
        if( ModelState.IsValid ){
            var username = HttpContext.User.Identity?.Name; 
            var loggedInUser = await _userManager.FindByNameAsync( username );

            var userDetails = await _applicationDbContext.UserDetails.Where( u => u.AspNetUserId == loggedInUser.Id ).FirstOrDefaultAsync();

            userDetails!.Firstname = updateUser.Firstname!;
            userDetails.Lastname = updateUser.Lastname!;
            userDetails.Birthdate = updateUser.Birthdate!;

            _applicationDbContext.Entry(userDetails).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();

            return Ok(new{
                Firstname = userDetails.Firstname,
                Lastname = userDetails.Lastname,
                Birthdate = userDetails.Birthdate
            });

        }
        else{
            return BadRequest();
        }
    }

    [HttpPut,Route("setBalance"),Authorize]
    public async Task<IActionResult> SetBalance(Decimal balanceAmount){
        var loggedInUser = await _userManager.FindByNameAsync( HttpContext.User.Identity?.Name );
        var userDetails = await _applicationDbContext.UserDetails.Where( u => u.AspNetUserId == loggedInUser.Id ).FirstOrDefaultAsync();

        userDetails!.Balance = balanceAmount;

        _applicationDbContext.Entry(userDetails).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();

        return Ok( new {
            message = "Successfully updated the balance",
            Balance = userDetails.Balance
            }
        );
    }

    [HttpDelete("{email}"),Authorize(Roles = "Super Administrator,Administrator")]
    public async Task<IActionResult> Delete( string email ){
        IdentityUser userToBeDeleted = await _userManager.FindByEmailAsync(email);

        
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