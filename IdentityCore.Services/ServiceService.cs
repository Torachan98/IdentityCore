using IdentityCore.Business;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IdentityCore.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceBusiness _serviceBusiness;

        public ServiceService (IServiceBusiness serviceBusiness)
        {
            _serviceBusiness = serviceBusiness;
        }

        public async Task<PaginationItems<ServiceDTO>> GetAllAsync(ServiceFetchRequest request)
        {
            return await _serviceBusiness.GetServices(request);
        }

        public async Task<ServiceDTO> GetByIdAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceDTO> CreateAsync(CreateOrUpdateServiceRequest request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Name service do not allow empty");
            }

            if (!string.IsNullOrEmpty(request.SignatureKey))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Key service do not allow empty");
            }

            return await _serviceBusiness.CreateServicesAsync(request);
        }

        public async Task<ServiceDTO> UpdateAsync(CreateOrUpdateServiceRequest request)
        {
            if (string.IsNullOrEmpty(request.GUID))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Id service do not allow empty");
            }

            return await _serviceBusiness.UpdateServicesAsync(request);
        }

        public async Task<bool> DeleteAsync(string guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Id do not allow empty");
            }

            return await _serviceBusiness.DeleteServicesAsync(guid);
        }
    }
}
