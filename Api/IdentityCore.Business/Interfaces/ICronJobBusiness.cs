namespace IdentityCore.Business.Interfaces
{
    public interface ICronJobBusiness
    {
        Task CleanBlackListAsync();
        Task ResetOTPAsync();
    }
}
