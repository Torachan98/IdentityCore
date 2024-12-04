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

        public async Task<PaginationItems<ServciceDTO>> GetAllAsync(ServiceFetchRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ServciceDTO> GetByIdAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public async Task<ObjectResult<ServciceDTO>> CreateAsync(CreateOrUpdateService request)
        {
            throw new NotImplementedException();
        }

        public async Task<ObjectResult<ServciceDTO>> UpdateAsync(CreateOrUpdateService request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string guid)
        {
            throw new NotImplementedException();
        }
    }
}
