using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using System;

namespace IdentityCore.Business.Interfaces
{
    public interface IRoleBusiness
    {
        Task<PaginationItems<RoleDTO>> GetRoles(RoleFetchRequest request);
        Task<RoleDTO> GetRoleByIdAsync(Guid guid);
        Task<RoleDTO> CreateRolesAsync(CreateOrUpdateRoleRequest request);
        Task<RoleDTO> UpdateRolesAsync(CreateOrUpdateRoleRequest request);
        Task<bool> DeleteRolesAsync(Guid guid);
    }
}
