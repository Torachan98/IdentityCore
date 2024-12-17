using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ObjectResponse<AuthenticationToken>> SignIn(SignInRequest signInRequest);
        Task<bool> SignOut();
        Task<AuthenticationToken> RenewToken(string refreshToken);
        Task<bool> ResetEmail(string email);
        Task<bool> ResetEmailConfirm(string email, string otpCode);
        Task<bool> ResetPassword(string email);
        Task<bool> ResetPasswordConfirm(string password, string otpCode);
        Task<bool> ConfirmOTP(string otpCode);
    }
}
