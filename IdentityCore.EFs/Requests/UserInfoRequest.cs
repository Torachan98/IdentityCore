using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.EFs.Requests
{
    public class UserInfoRequest
    {
        public string Jwt { get; set; } = string.Empty;
    }

    public class UserInfoResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<RoleEnum> Roles { get; set; }
    }
}
