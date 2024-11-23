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
        private readonly IAuthenticationBusiness _authenticationBusiness;
        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IAuthenticationBusiness authenticationBusiness)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationBusiness = authenticationBusiness;
        }

        public async Task<ObjectResponse<AuthenticationToken>> SignIn(SignInRequest signInRequest)
        {
            var errors = Validator.ValidateRequiredProperties(signInRequest);
            if (errors.Any())
            {
                return new ObjectResponse<AuthenticationToken>()
                {
                    Message = JsonConvert.SerializeObject(errors),
                };
            }

            var result = await _authenticationBusiness.SignInAsync(signInRequest);

            return result;
        }

        public async Task<bool> SignOut()
        {
            var userDto = _httpContextAccessor.HttpContext.Items["User"] as UserDTO;
            if(userDto == null)
            {
                return false;
            }

            return await _authenticationBusiness.SignOutAsync(userDto);
        }

        public async Task<AuthenticationToken?> RenewToken(string refreshToken)
        {
            return await _authenticationBusiness.RenewTokenAsync(refreshToken);
        }

        public async Task<bool> ConfirmOTP(string otpCode)
        {
            return await _authenticationBusiness.ConfirmOTPAsync(otpCode);
        }
    }
}
