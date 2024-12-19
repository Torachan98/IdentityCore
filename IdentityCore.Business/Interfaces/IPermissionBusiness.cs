using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Business.Interfaces
{
    public interface IPermissionBusiness
    {
        Task<PaginationItems<PermissionDTO>> GetPermissionsAsync(PermissionFetchRequest request);
        Task<PermissionDTO> CreatePermissionsAsync(CreateOrUpdatePermissionRequest request);
        Task<PermissionDTO> UpdatePermissionsAsync(CreateOrUpdatePermissionRequest request);
        Task<bool> DeletePermissionsAsync(string guid);
    }
}
