using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;

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

        public Task<ObjectResult<PermissionDTO>> CreateAsync(CreateOrUpdatePermissionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<PermissionDTO> GetByIdAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<ObjectResult<PermissionDTO>> UpdateAsync(CreateOrUpdatePermissionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
