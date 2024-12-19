using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Business.Interfaces
{
    public interface IRoleBusiness
    {
        Task<PaginationItems<RoleDTO>> GetRoles(RoleFetchRequest request);
        Task<RoleDTO> CreateRolesAsync(CreateOrUpdateRoleRequest request);
        Task<RoleDTO> UpdateRolesAsync(CreateOrUpdateRoleRequest request);
        Task<bool> DeleteRolesAsync(string guid);
    }
}
