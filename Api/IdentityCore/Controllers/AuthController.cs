using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCore.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string UrlRoot = "api/auth";
        private const string SignInRoute = UrlRoot + "/signin";
        private const string RegistrationRoute = UrlRoot + "/registration";
        private const string ForgotPasswordRoute = UrlRoot + "/forgot-password";       
        private const string ConfirmOTPRoute = UrlRoot + "/confirm-otp";
        private const string ReSentOTPRoute = UrlRoot + "/re-sent-otp";
        private const string ResetPasswordUserRoute = UrlRoot + "/reset-password";
        private const string ResetEmailUserRoute = UrlRoot + "/reset-email";
        private const string ResetEmaiConfirmlUserRoute = UrlRoot + "/confirm-reset-email";
        private const string RenewTokenRoute = UrlRoot + "/renew-token";
        private const string SignOutRoute = UrlRoot + "/signout";

        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IUserService userService, IAuthenticationService authenticationService) 
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route(RegistrationRoute)]
        [DisableCors]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser([FromBody] CreateOrUpdateUserRequest createUserRequest)
        {
            var result = await _userService.CreateAsync(createUserRequest);
            return Ok(result);
        }    
        
        [HttpPost]
        [Route(ConfirmOTPRoute)]
        [DisableCors]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmOTP([FromBody] OTPRequest otpRequest)
        {
            if (string.IsNullOrEmpty(otpRequest.OTPCode))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "OTP invalid");
            }

            var result = await _authenticationService.ConfirmOTP(otpRequest.OTPCode);

            if (!result)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "OTP has expired or not existed");
            }

            return Ok(result);
        }

        [HttpPost]
        [Route(ReSentOTPRoute)]
        [DisableCors]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReSentOTP([FromBody] string email)
        {
            await _authenticationService.ReSentOTP(email);
            return Ok(true);
        }

        [HttpPost]
        [Route(ForgotPasswordRoute)]
        [DisableCors]
        [Attributes.Authorize]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            if (forgotPasswordRequest == null) {
                return NotFound();
            }

            var result = await _userService.ForgotPasswordAsync(forgotPasswordRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route(SignInRoute)]
        [DisableCors]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ObjectResponse<AuthenticationToken>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
        {
            var result = await _authenticationService.SignIn(signInRequest);

            if(result.Item != null && result.Item.RefreshToken != null)
            {
                Response.Cookies.Append("refreshToken", result.Item.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Path = UrlRoot,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    }
                );
            }

            return Ok(result);
        }

        [HttpPost]
        [Route(RenewTokenRoute)]
        [DisableCors]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RenewToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest,"Refresh token invalid");
            }

            var result = await _authenticationService.RenewToken(refreshToken);

            if (string.IsNullOrEmpty(result.RefreshToken))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Refresh token invalid. Please retry it");
            }

            Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,  
                    SameSite = SameSiteMode.None,
                    Path = UrlRoot,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                }
            );

            return Ok(result.AccessToken);
        }

        [HttpPost]
        [Route(ResetEmailUserRoute)]
        [DisableCors]
        public async Task<IActionResult> ResetEmail([FromQuery] string emailAddress)
        {
            return Ok(await _authenticationService.ResetEmail(emailAddress));
        }

        [HttpPost]
        [Route(ResetEmaiConfirmlUserRoute)]
        [DisableCors]
        [AllowAnonymous]
        public async Task<IActionResult> ResetEmailConfirm([FromQuery] string emailAddress,string otpCode)
        {
            return Ok(await _authenticationService.ResetEmailConfirm(emailAddress, otpCode));
        }

        [HttpPost]
        [Route(ResetPasswordUserRoute)]
        [DisableCors]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword()
        {
            return Ok();
        } 

        [HttpPost]
        [Route(SignOutRoute)]
        [DisableCors]
        [Attributes.Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Signout()
        {
            return Ok(await _authenticationService.SignOut());
        }
    }
}
