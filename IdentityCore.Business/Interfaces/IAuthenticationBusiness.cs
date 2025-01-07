using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Business.Interfaces
{
    public interface IAuthenticationBusiness
    {
        Task<ObjectResponse<AuthenticationToken>> SignInAsync(SignInRequest signInRequest);
        Task<bool> SignOutAsync(UserDTO userDto,string accessToken);
        Task<AuthenticationToken> RenewTokenAsync(string refreshToken);
        Task<bool> ConfirmOTPAsync(string otpCode);
        Task<bool> ResetEmailAsync(string email);
        Task<bool> ResetEmailConfirmAsync(string email, string otpCode);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ResetPasswordConfirmAsync(string password, string otpCode);
    }
}
