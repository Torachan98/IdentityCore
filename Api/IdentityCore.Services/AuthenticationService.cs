using AutoMapper;
using Google.Apis.Drive.v3.Data;
using IdentityCore.Business;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Helpers;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IdentityCore.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IAuthenticationBusiness authenticationBusiness, 
            IUserBusiness userBusiness,
            IEmailBusiness emailBusiness)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _authenticationBusiness = authenticationBusiness;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
        }

        public async Task<ObjectResponse<AuthenticationToken>> SignIn(SignInRequest signInRequest)
        {
            var errors = Validator.ValidateRequiredProperties(signInRequest);
            if (errors.Any())
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, JsonConvert.SerializeObject(errors));
            }

            var result = await _authenticationBusiness.SignInAsync(signInRequest);

            return result;
        }

        public async Task<bool> SignOut()
        {
            var userDto = _httpContextAccessor.HttpContext!.Items["User"] as UserDTO;
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();

            if(userDto == null)
            {
                return false;
            }

            return await _authenticationBusiness.SignOutAsync(userDto, accessToken);
        }

        public async Task<AuthenticationToken> RenewToken(string refreshToken)
        {
            return await _authenticationBusiness.RenewTokenAsync(refreshToken);
        }

        public async Task<bool> ConfirmOTP(string otpCode)
        {
            return await _authenticationBusiness.ConfirmOTPAsync(otpCode);
        }

        public async Task<string> ResetEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Email is not empty");
            }

            await _authenticationBusiness.ResetEmailAsync(email);
            return "OTP has been sent into email";
        }

        public async Task<string> ResetEmailConfirm(string email, string otpCode)
        {
            await _authenticationBusiness.ResetEmailConfirmAsync(email, otpCode);
            return "Confirmed";
        }

        public async Task<string> ResetPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Email is not empty");
            }

            await _authenticationBusiness.ResetPasswordAsync(email);
            return "OTP has been sent into email";
        }

        public async Task<string> ResetPasswordConfirm(string password, string otpCode)
        {
            if (!string.IsNullOrEmpty(password))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Email is not empty");
            }

            if (!string.IsNullOrEmpty(otpCode))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "OTP is not empty");
            }

            await _authenticationBusiness.ResetPasswordConfirmAsync(password, otpCode);
            return "Confirmed";
        }

        public async Task ReSentOTP()
        {
            var userDto = _httpContextAccessor.HttpContext!.Items["User"] as UserDTO;
            if (userDto == null)
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "User not found");
            }


            var userEntity = await _userBusiness.GetUserById(userDto.GUID);

            userEntity.OTPCode = _emailBusiness.GenerateOTP(GlobalConst.OTP.SizeCode);
            userEntity.OTPLifeTime = DateTime.UtcNow.AddMinutes(GlobalConst.OTP.LifeTimeMinute);

            var userPayload = _mapper.Map<UserDTO>(userEntity);

            await _userBusiness.UpdateUserAsync(userPayload);

            await _emailBusiness.SendMailAsync(userPayload, TemplateEmailType.OTP);
        }
    }
}
