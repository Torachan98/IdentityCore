using IdentityCore.EFs;

namespace IdentityCore.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IdentityContext identityContext { get; }
        void Commit();
        Task<int> CommitAsync();
        Task DisposeAsync();
    }
}
