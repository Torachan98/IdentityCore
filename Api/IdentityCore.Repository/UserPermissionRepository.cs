using IdentityCore.EFs.Entities;
using IdentityCore.Repository.Base;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;

namespace IdentityCore.Repository
{
    public class UserPermissionRepository : Repository<UserPermissionEntity>, IUserPermissionRepository
    {
        public UserPermissionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
