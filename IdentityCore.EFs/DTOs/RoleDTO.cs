using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Requests;

namespace IdentityCore.EFs.DTOs
{
    public class RoleDTO
    {
        public string? GUID { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
    }

    public class RoleEnum
    {
        public Role Role { get; set; }
        public string? Description { get; set; }
        public List<PermissionEnum>? Permissions { get; set; } 
    }

    public class CreateOrUpdateRoleRequest
    {
        public string? GUID { get; set; }
        public required string RoleName { get; set; }
        public string? Description { get; set; }
        public bool IsLock { get; set; }
    }

    public class RoleFetchRequest: FetchParams
    {
        public bool? IsLock { get; set; }
    }
}
