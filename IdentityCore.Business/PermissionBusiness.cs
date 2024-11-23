using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;

namespace IdentityCore.Business
{
    public class PermissionBusiness : BaseBusiness, IPermissionBusiness
    {
        public Task<List<PermissionDTO>> CreatePermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<PermissionDTO>> GetPermissions()
        {
            throw new NotImplementedException();
        }

        public Task<List<PermissionDTO>> UpdatePermissionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
