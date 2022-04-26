
namespace helloAPI.Controllers;

[ApiController,Route("users")]
public class UsersController:ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = null!;
    private readonly SignInManager<IdentityUser> _signInManager = null!;
    private readonly ApplicationDbContext _applicationDbContext = null!;

    private readonly IConfiguration _configuration = null!;
    private readonly IHostEnvironment _hostEnvironment = null!;

    private readonly SendGridService _sendGridService = null!;
    private readonly TwilioService _twilioService = null!;


    public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext applicationDbContext, IConfiguration configuration, IHostEnvironment hostEnvironment, SendGridService sendGridService, TwilioService twilioService ){
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationDbContext = applicationDbContext;
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
        _sendGridService = sendGridService;
        _twilioService = twilioService;
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
///<remarks>
///Note: the user will not be able to login until the email and phonenumber
///  is both confirmed.
///  Also for DEMO purposes, the confirmation token/code for email/phone respectively
///  are returned in this endpoint. They can be used confirmation.
///
/// Returns: The user id of the registered user. 
///  
///  Sample Return:
/// 
///{
///     
/// User: {
/// 
///    Id: "6bb323e4-72e7-4cb8-b987-312d9fc87864"
/// 
///},
/// 
///SMScode: "123456",
/// 
///emailcode: "CfDJ8H1eyD1F9kVLs0nIhefMBrz71RO3u0TQLeg%2F9ciK3cDTEtKUxGPfxdWJ90wN5U9WUJ5tUbaZo0x5Unr04d4AaA0QMiTy7IeZI6xUUJorrvNYdHhDTjxj5OuopfQCKMnDJuZwbdLlkUl4XBpT1pi%2Frmd7VQfKHuIvhwXwOK6Uaxd4d%2BM8z7DAFgdnAzhseJprNx9owVHy15I0zkNJbt80J3hFbLu1WRxPXf72iYlrByBSGZGGiwvhB0JbxpuQ4RHxLw"
/// 
///}
/// 
///</remarks>
    [HttpPost,Route("register")]
    public  async Task<IActionResult> registerUser( RegisterUser registerUser){

        var user = new IdentityUser { UserName = registerUser.Email, Email = registerUser.Email,PhoneNumber = registerUser.Phone };
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

            var code2FA = await _userManager.GenerateChangePhoneNumberTokenAsync( user, registerUser.Phone );

            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync( user );
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Users", new { token = confirmEmailToken, email = user.Email }, Request.Scheme);

            //send confirmation email
            /*
            var apiKey = _configuration["Sendgrid:Api_key"];
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["Sendgrid:Mailfrom"], _configuration["Sendgrid:Mailfromname"]),
                Subject = "Confirm your email on helloAPI",
                PlainTextContent = "you have registered via the hello API, please click the link below to confirm your email address \n\n" + confirmationLink
            };

            msg.AddTo(new EmailAddress(registerUser.Email));

            await client.SendEmailAsync(msg);
            */

            var response = await _sendGridService.Send( registerUser.Email!, "Confirm your email on helloAPI", "you have registered via the hello API, please click the link below to confirm your email address \n\n" + confirmationLink );

            
            
            if( _configuration.GetSection("Twilio:VerifiedNumbers").Get<string[]>().Contains( registerUser.Phone!) )
                await _twilioService.Send( registerUser.Phone!, "Your confirmation code is: " + code2FA );            

            return Ok( new{
                 message = "you have successfully registered. please confirm your email and your phone number",
                 User = new {
                     Id = user.Id
                 },
                 SMScode = code2FA,
                 emailcode = confirmEmailToken
            });


            
        }
        else{
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Registration failed: " + string.Join("\n", result.Errors.Select(e => e.Description).ToList() )  } );
        }
        
        
    }

///<remarks>
/// Used after completing registration.
/// 
/// This endpoint probably is not gonna be used if the implementation is
/// that which requires the user to click a certain link
///
/// for the parameters, the Id is returned upon registration
///
/// the token is emailed
///</remarks>
    [HttpGet,Route("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail( string token, string Id ){
        var confirmingUser = await _userManager.FindByIdAsync(Id);
        if( confirmingUser == null )
            return BadRequest();
        
        var result = await _userManager.ConfirmEmailAsync(confirmingUser, token );
        return result.Succeeded ? Ok( new{ message = "Successfully confirmed your email" }) : BadRequest();
        
    }

///<remarks>
/// Used after completing registration.
///
/// for the parameters, the Id is returned upon registration
///
/// the code is sent via sms to the registered phone number
///</remarks>
    [HttpPost,Route("confirmPhonenumber")]
    public async Task<IActionResult> ConfirmPhoneNumber( string smscode, string Id ){
        var confirmingUser = await _userManager.FindByIdAsync( Id );

        if( confirmingUser == null )
            return BadRequest();

        var result = await _userManager.ChangePhoneNumberAsync( confirmingUser, confirmingUser.PhoneNumber, smscode );

        if( result.Succeeded ){
            await _userManager.SetTwoFactorEnabledAsync( confirmingUser, true );
            return Ok( new { message = "Successfully confirmed phone number" } );
        }
        else{
            return StatusCode( StatusCodes.Status500InternalServerError, new { message = "Error confirming phone number" } );
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

///<summary>Login with email and password, sends an SMS containing a 1-time password to the registered phone number</summary>
/// <remarks>
/// for this example, since i am only using a trial twilio account, the OTP is 
/// returned with this endpoint as well. You can use it to login on the /users/loginOTP endpoint
/// </remarks>
    [HttpPost,Route("login")]
    public async Task<IActionResult> loginUser( LoginUser loginUser){

        
            var result = await _signInManager.PasswordSignInAsync( loginUser.email, loginUser.password, isPersistent: false, lockoutOnFailure: true);

            var loggedInUser = await _userManager.FindByEmailAsync(loginUser.email);

            var role = await _userManager.GetRolesAsync(loggedInUser);

            if( result.RequiresTwoFactor || role.Contains("Super Administrator") ){


                var code2FA = await _userManager.GenerateTwoFactorTokenAsync( loggedInUser, "Phone" );

                if( _configuration.GetSection("Twilio:VerifiedNumbers").Get<string[]>().Contains( loggedInUser.PhoneNumber) )
                    await _twilioService.Send(loggedInUser.PhoneNumber,"Your 1 time password is: " + code2FA );
                

                return Ok(new { 
                    User = new{
                        Id = loggedInUser.Id,
                        Phonenumber = loggedInUser.PhoneNumber,
                        OTP = code2FA
                    } } 
                    );
            }
            else{

                if( loggedInUser.PhoneNumberConfirmed == false ){
                    var code2FA = await _userManager.GenerateChangePhoneNumberTokenAsync( loggedInUser, loggedInUser.PhoneNumber );
                    if( _configuration.GetSection("Twilio.VerifiedNumbers").Get<string[]>().Contains( loggedInUser.PhoneNumber) ) 
                        await _twilioService.Send( loggedInUser.PhoneNumber, "Your confirmation code is: " + code2FA );
                    else
                        await _sendGridService.Send( loggedInUser.Email!, "Confirm Your Phone on helloAPI", "This should be sent via phone but for testing purposes, this arives via email \n\n\n Your confirmation code is: " + code2FA );
                }

                if( loggedInUser.EmailConfirmed == false ){
                    var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync( loggedInUser );
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Users", new { token = confirmEmailToken, email = loggedInUser.Email }, Request.Scheme);
                    var response = await _sendGridService.Send( loggedInUser.Email!, "Confirm your email on helloAPI", "you have registered via the hello API, please click the link below to confirm your email address \n\n" + confirmationLink );

                }

            }
            
        
        

        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Login Failed"  } );
    }

/// <summary>
/// returns a bearer token used for request Authorization 
/// </summary>
    [HttpPost,Route("loginOTP")]
    public async Task<IActionResult> loginSMS2Fa( string Id, string smscode ){
        var user = await _userManager.FindByIdAsync(Id);

        var verified = await _userManager.VerifyTwoFactorTokenAsync( user, "Phone", smscode );

        if( verified ){
            IdentityUser loggedInUser = user;
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
        else{
            return BadRequest(new {message = "invalid OTP"});
        }
    }

/// <remarks>
/// upload via form data field named "profilePicture" (multipart/form-data)
/// </remarks>
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

///<summary>used to request for a new auth token after it expires</summary>
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