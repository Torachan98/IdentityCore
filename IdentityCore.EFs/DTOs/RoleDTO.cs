using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Requests;
using System.Runtime.Serialization;

namespace IdentityCore.EFs.DTOs
{
    public class RoleDTO
    {
        public string? GUID { get; set; }
        public string? Name { get; set; }
        public Role Role { get; set; }

        public string? Description { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = new List<PermissionDTO>();
    }

    public class RoleResponse
    {
        public string Role { get; set; }
        public int Value { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class RoleEnum
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int Value { get; set; }
    }

    public class CreateOrUpdateRoleRequest
    {
        public string? GUID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsLock { get; set; }
    }

    public class RoleFetchRequest: FetchParams
    {
        public bool? IsLock { get; set; }
    }
}
