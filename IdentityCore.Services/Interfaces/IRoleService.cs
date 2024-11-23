using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Services.Interfaces
{
    public interface IRoleService
    {
        Task<string> CreateRoleAsync();
        Task<string> UpdateRoleAsync();
        Task<string> DeleteRoleAsync();
    }
}
