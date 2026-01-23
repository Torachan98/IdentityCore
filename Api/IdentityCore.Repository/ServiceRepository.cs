using IdentityCore.EFs.Entities;
using IdentityCore.Repository.Base;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;

namespace IdentityCore.Repository
{
    public class ServiceRepository : Repository<ServiceEntity>, IServiceRepository
    {
        public ServiceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
