using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Requests;
using System;
using System.Runtime.Serialization;

namespace IdentityCore.EFs.DTOs
{
    public class RoleDTO
    {
        public Guid GUID { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool IsLock { get; set; }

        public string? Description { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = new List<PermissionDTO>();
    }

    public class RoleResponse
    {
        public Guid? GUID { get; set; }
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
        public Guid? GUID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsLock { get; set; }

        public List<Guid> Permissions { get; set; } = new List<Guid>();
    }

    public class RoleFetchRequest: FetchParams
    {
        public bool? IsLock { get; set; }
    }
}
