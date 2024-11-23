using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Business.Interfaces
{
    public interface IRoleBusiness
    {
        Task<List<RoleDTO>> GetRoles();
        Task<List<RoleDTO>> CreateRolesAsync();
        Task<List<RoleDTO>> UpdateRolesAsync(UserRequest userRequest);
        Task<bool> DeleteRolesAsync(UserDTO user);
    }
}
