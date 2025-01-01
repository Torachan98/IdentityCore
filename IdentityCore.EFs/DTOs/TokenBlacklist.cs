using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.EFs.DTOs
{
    public class TokenBlacklist
    {
        public string Token { get; set; } = string.Empty;
        public DateTime DateExpired { get; set; }
    }
}
