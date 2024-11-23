using IdentityCore.EFs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ObjectResponse<AuthenticationToken>> SignIn(SignInRequest signInRequest);
        Task<bool> SignOut();
        Task<AuthenticationToken?> RenewToken(string refreshToken);
        Task<bool> ConfirmOTP(string otpCode);
    }
}
