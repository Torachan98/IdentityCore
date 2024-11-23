using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Business
{
    public class RoleBusiness : BaseBusiness, IRoleBusiness
    {
        public Task<List<RoleDTO>> CreateRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRolesAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<List<RoleDTO>> GetRoles()
        {
            throw new NotImplementedException();
        }

        public Task<List<RoleDTO>> UpdateRolesAsync(UserRequest userRequest)
        {
            throw new NotImplementedException();
        }
    }
}
