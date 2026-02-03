using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ObjectResponse<AuthenticationToken>> SignIn(SignInRequest signInRequest);
        Task<bool> SignOut();
        Task ReSentOTP(Guid guid);
        Task<AuthenticationToken> RenewToken(string refreshToken);
        Task<string> ResetEmail(string email);
        Task<string> ResetEmailConfirm(string email, string otpCode);
        Task<string> ResetPassword(string email);
        Task<string> ResetPasswordConfirm(string password, string otpCode);
        Task<bool> ConfirmOTP(string otpCode);
    }
}
