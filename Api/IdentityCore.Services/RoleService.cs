using IdentityCore.Business;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IdentityCore.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleBusiness _roleBusiness;
        public RoleService(IRoleBusiness roleBusiness) 
        {
            _roleBusiness = roleBusiness;
        }

        public async Task<PaginationItems<RoleDTO>> GetAllAsync(RoleFetchRequest request)
        {
            return await _roleBusiness.GetRoles(request);
        }

        public async Task<RoleDTO> GetByIdAsync(Guid guid)
        {
            return await _roleBusiness.GetRoleByIdAsync(guid);
        }

        public async Task<RoleDTO> CreateAsync(CreateOrUpdateRoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Name role do not allow empty");
            }

            return await _roleBusiness.CreateRolesAsync(request);
        }

        public async Task<RoleDTO> UpdateAsync(CreateOrUpdateRoleRequest request)
        {
            if (!request.GUID.HasValue)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Id role do not allow empty");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Name role do not allow empty");
            }

            return await _roleBusiness.UpdateRolesAsync(request);
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            //if (!string.IsNullOrEmpty(guid))
            //{
            //    throw new FriendlyException(StatusCodes.Status400BadRequest, "Id do not allow empty");
            //}

            return await _roleBusiness.DeleteRolesAsync(guid);
        }
    }
}
