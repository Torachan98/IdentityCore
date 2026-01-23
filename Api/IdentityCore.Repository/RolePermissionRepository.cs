using IdentityCore.EFs.Entities;
using IdentityCore.Repository.Base;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;

namespace IdentityCore.Repository
{
    public class RolePermissionRepository : Repository<RolePermissionEntity>, IRolePermissionRepository
    {
        public RolePermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
