using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.EFs
{
    public class AuthenticationToken
    {
        public string? AccessToken { get; set; }

        [IgnoreDataMember]
        public string? RefreshToken { get; set; }
        public int Step { get; set; }
    }
}
