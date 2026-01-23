using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Business.Interfaces
{
    public interface IServiceBusiness
    {
        Task<PaginationItems<ServiceDTO>> GetServices(ServiceFetchRequest request);
        Task<ServiceDTO> CreateServicesAsync(CreateOrUpdateServiceRequest request);
        Task<ServiceDTO> UpdateServicesAsync(CreateOrUpdateServiceRequest request);
        Task<List<AssigningService>> AssigningUserServiceAsync(List<AssigningService> request, int userId);
        Task<bool> DeleteServicesAsync(Guid guid);
    }
}
