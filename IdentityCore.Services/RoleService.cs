using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;

namespace IdentityCore.Services
{
    public class RoleService : IRoleService
    {
        public Task<ObjectResult<RoleDTO>> CreateAsync(CreateOrUpdateRoleRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationItems<RoleDTO>> GetAllAsync(RoleFetchRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<RoleDTO> GetByIdAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<ObjectResult<RoleDTO>> UpdateAsync(CreateOrUpdateRoleRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
