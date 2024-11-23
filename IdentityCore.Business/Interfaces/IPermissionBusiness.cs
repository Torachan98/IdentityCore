using IdentityCore.EFs.DTOs;

namespace IdentityCore.Business.Interfaces
{
    public interface IPermissionBusiness
    {
        Task<List<PermissionDTO>> GetPermissions();
        Task<List<PermissionDTO>> CreatePermissionsAsync();
        Task<List<PermissionDTO>> UpdatePermissionsAsync();
        Task<bool> DeletePermissionsAsync();
    }
}
