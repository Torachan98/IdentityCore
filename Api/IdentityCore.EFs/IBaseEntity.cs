using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.EFs
{
    public interface IBaseEntity
    {
        Guid GUID { get; set; }
        DateTimeOffset DateCreated { get; set; }
        DateTimeOffset DateModified { get; set; }
        bool IsDeleted { get; set; }
    }
}
