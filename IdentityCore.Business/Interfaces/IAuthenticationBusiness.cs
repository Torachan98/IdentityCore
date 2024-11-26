using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Business.Interfaces
{
    public interface IAuthenticationBusiness
    {
        Task<ObjectResponse<AuthenticationToken>> SignInAsync(SignInRequest signInRequest);
        Task<bool> SignOutAsync(UserDTO userDto);
        Task<AuthenticationToken?> RenewTokenAsync(string refreshToken);
        Task<bool> ValidateToken();
        Task<bool> ConfirmOTPAsync(string otpCode);

        Task<bool> ResetEmail(string email);

        Task<string> ResetEmailConfirm(string email, string otpCode);

        Task<bool> ResetPassword(string email);

        Task<string> ResetPasswordConfirm(string password, string otpCode);
    }
}
