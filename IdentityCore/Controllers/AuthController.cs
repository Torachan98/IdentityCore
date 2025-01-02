using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateOrUpdateUserRequest createUserRequest)
        {
            var result = await _userService.CreateAsync(createUserRequest);
            return Ok(result);
        }    
        
        [HttpPost]
        [Route(ConfirmOTPRoute)]
        [AllowAnonymous]
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
        [Route(ForgotPasswordRoute)]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
        {
            var result = await _authenticationService.SignIn(signInRequest);

            if (!string.IsNullOrEmpty(result.Message))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, result.Message);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route(RenewTokenRoute)]
        [AllowAnonymous]
        public async Task<IActionResult> RenewToken([FromBody] AuthenticationToken authenticationToken)
        {
            if (string.IsNullOrEmpty(authenticationToken.RefreshToken))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest,"Refresh token invalid");
            }

            return Ok(await _authenticationService.RenewToken(authenticationToken.RefreshToken));
        }

        [HttpPost]
        [Route(ResetEmailUserRoute)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetEmail([FromQuery] string emailAddress)
        {
            return Ok(await _authenticationService.ResetEmail(emailAddress));
        }

        [HttpPost]
        [Route(ResetEmaiConfirmlUserRoute)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetEmailConfirm([FromQuery] string emailAddress,string otpCode)
        {
            return Ok(await _authenticationService.ResetEmailConfirm(emailAddress, otpCode));
        }

        [HttpPost]
        [Route(ResetPasswordUserRoute)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword()
        {
            return Ok();
        } 

        [HttpPost]
        [Route(SignOutRoute)]
        [Attributes.Authorize]
        public async Task<IActionResult> Signout()
        {
            return Ok(await _authenticationService.SignOut());
        }
    }
}
