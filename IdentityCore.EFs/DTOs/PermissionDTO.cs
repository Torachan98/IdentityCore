using IdentityCore.EFs.Requests;

namespace IdentityCore.EFs.DTOs
{
    public class PermissionDTO
    {
        public Guid Guid { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public RoleDTO Role { get; set; }
    }

    public class PermissionEnum
    {       
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; }
    }

    public class UserRolePermissionDTO
    {
        public int UserRolePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }

    public class PermissionFetchRequest: FetchParams
    {
       
    }

    public class CreateOrUpdatePermissionRequest
    {
        public Guid? GUID { get; set; }
        public int Value { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
