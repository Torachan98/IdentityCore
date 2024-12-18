using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IdentityCore.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionBusiness _permissionBusiness;
        public PermissionService(IPermissionBusiness permissionBusiness) 
        {
            _permissionBusiness = permissionBusiness;
        }

        public async Task<PaginationItems<PermissionDTO>> GetAllAsync(PermissionFetchRequest request)
        {
            return await _permissionBusiness.GetPermissionsAsync(request);
        }

        public Task<PermissionDTO> GetByIdAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public async Task<ObjectResult<PermissionDTO>> CreateAsync(CreateOrUpdatePermissionRequest request)
        {
            if (!string.IsNullOrEmpty(request.Name)) 
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Name permission do not allow empty");
            }

            return new ObjectResult<PermissionDTO>()
            {
                Item = await _permissionBusiness.CreatePermissionsAsync(request),
            };
        }

        public async Task<ObjectResult<PermissionDTO>> UpdateAsync(CreateOrUpdatePermissionRequest request)
        {
            if (!string.IsNullOrEmpty(request.GUID))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Id permission do not allow empty");
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Name permission do not allow empty");
            }

            return new ObjectResult<PermissionDTO>()
            {
                Item = await _permissionBusiness.UpdatePermissionsAsync(request),
            };
        }

        public async Task<bool> DeleteAsync(string guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Id do not allow empty");
            }

            return await _permissionBusiness.DeletePermissionsAsync(guid);
        }
    }
}
