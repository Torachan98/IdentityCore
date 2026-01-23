using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Services.Interfaces
{
    public interface IRoleService: IBaseService<RoleDTO, CreateOrUpdateRoleRequest, RoleFetchRequest>
    {

    }
}
