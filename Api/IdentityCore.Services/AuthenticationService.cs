using AutoMapper;
using Google.Apis.Drive.v3.Data;
using IdentityCore.Business;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Helpers;
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

        private readonly IFCMService _fcmService;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IAuthenticationBusiness authenticationBusiness, 
            IUserBusiness userBusiness,
            IEmailBusiness emailBusiness,
            IFCMService fcmService)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _authenticationBusiness = authenticationBusiness;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
            _fcmService = fcmService;
        }

        public async Task<ObjectResponse<AuthenticationToken>> SignIn(SignInRequest signInRequest)
        {
            var errors = Validator.ValidateRequiredProperties(signInRequest);
            if (errors.Any())
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, JsonConvert.SerializeObject(errors));
            }

            var result = await _authenticationBusiness.SignInAsync(signInRequest);

            var anotherUser = await _userBusiness.GetAllAsync(s => !s.IsDeleted);

            var userCurrent = anotherUser.FirstOrDefault(s => s.UserName == signInRequest.UserName || s.Email == signInRequest.UserName);

            foreach (var usr in anotherUser.Where(s => s.UserName != signInRequest.UserName || s.Email != signInRequest.UserName)) 
            {
                if (!string.IsNullOrEmpty(usr.FcmToken))
                {
                    await _fcmService.SendAsync(usr.FcmToken, "Login", $"User {userCurrent!.UserName} has online");
                }
            }

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

            var isSignout = await _authenticationBusiness.SignOutAsync(userDto, accessToken);

            if (isSignout) 
            {
                var anotherUser = await _userBusiness.GetAllAsync(s => !s.IsDeleted && s.GUID != userDto.GUID);
                foreach (var usr in anotherUser)
                {
                    if (!string.IsNullOrEmpty(usr.FcmToken))
                    {
                        await _fcmService.SendAsync(usr.FcmToken, "Logout", $"User {userDto!.UserName} has offline");
                    }
                }
            }

            return isSignout;
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

        public async Task ReSentOTP(string email)
        {
            var isValid = Utility.IsValidEmail(email);
            if (!isValid)
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "Please input an email");
            }

            var userEntity = await _userBusiness.GetUserByEmail(email);

            if (userEntity == null)
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "User not found");
            }

            userEntity.OTPCode = _emailBusiness.GenerateOTP(GlobalConst.OTP.SizeCode);
            userEntity.OTPLifeTime = DateTime.UtcNow.AddMinutes(GlobalConst.OTP.LifeTimeMinute);

            var userPayload = _mapper.Map<UserDTO>(userEntity);

            await _userBusiness.UpdateUserAsync(userPayload);

            await _emailBusiness.SendMailAsync(userPayload, TemplateEmailType.OTP);
        }
    }
}
