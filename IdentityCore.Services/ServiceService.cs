using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;

namespace IdentityCore.Services
{
    public class ServiceService : IServiceService
    {
        public ServiceService ()
        {

        }

        public async Task<PaginationItems<ServiceDTO>> GetAllAsync(ServiceFetchRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceDTO> GetByIdAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public async Task<ObjectResult<ServiceDTO>> CreateAsync(CreateOrUpdateServiceRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ObjectResult<ServiceDTO>> UpdateAsync(CreateOrUpdateServiceRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string guid)
        {
            throw new NotImplementedException();
        }
    }
}
