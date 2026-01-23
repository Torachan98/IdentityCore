using IdentityCore.EFs.Entities;
using IdentityCore.Repository.Base;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;

namespace IdentityCore.Repository
{
    public class PermissionRepository : Repository<PermissionEntity>, IPermissionRepository
    {
        public PermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
