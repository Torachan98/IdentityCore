using IdentityCore.EFs.Entities;
using IdentityCore.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Repository.Interfaces
{
    public interface IUserRolePermissionRepository : IRepository<UserRolePermissionEntity>
    {
    }
}
