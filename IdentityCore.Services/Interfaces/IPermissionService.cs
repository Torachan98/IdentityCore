using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<string> CreatePermissionAsync();
        Task<string> UpdatePermissionAsync();
        Task<string> DeletePermissionAsync();
    }
}
