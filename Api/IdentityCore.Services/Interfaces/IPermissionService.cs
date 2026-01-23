using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Services.Interfaces
{
    public interface IPermissionService: IBaseService<PermissionDTO, CreateOrUpdatePermissionRequest, PermissionFetchRequest>
    {

    }
}
