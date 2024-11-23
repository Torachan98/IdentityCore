using IdentityCore.EFs;

namespace IdentityCore.Repository.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        public IdentityContext identityContext { get; }

        public UnitOfWork(IdentityContext dbContext)
        {
            identityContext = dbContext;
        }

        public Task<int> CommitAsync()
        {
            return identityContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            identityContext.Dispose();
        }

        public void Commit()
        {
            identityContext.SaveChanges();
        }

        public async Task DisposeAsync()
        {
            await identityContext.DisposeAsync();
        }
    }
}
