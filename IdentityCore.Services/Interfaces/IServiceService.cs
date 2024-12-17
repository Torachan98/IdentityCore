using IdentityCore.EFs.DTOs;

namespace IdentityCore.Services.Interfaces
{
    public interface IServiceService: IBaseService<ServiceDTO, CreateOrUpdateServiceRequest, ServiceFetchRequest>
    {
    }
}
