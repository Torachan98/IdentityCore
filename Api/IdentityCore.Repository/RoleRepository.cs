using IdentityCore.EFs.Entities;
using IdentityCore.Repository.Base;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;

namespace IdentityCore.Repository
{
    public class RoleRepository : Repository<RoleEntity>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
