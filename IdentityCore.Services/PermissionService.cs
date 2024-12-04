using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Services
{
    public class PermissionService : IPermissionService
    {
        public Task<ObjectResult<PermissionDTO>> CreateAsync(CreateOrUpdatePermissionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationItems<PermissionDTO>> GetAllAsync(PermissionFetchRequest request)
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
