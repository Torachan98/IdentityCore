using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Enums;
using System.Runtime.Serialization;

namespace IdentityCore.EFs.DTOs
{
    public class PermissionDTO
    {
        public PermissionType PermissionType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [IgnoreDataMember]
        public UserRolePermissionDTO UserRolePermissions { get; set; }


    }

    public class PermissionEnum
    {
        public PermissionType PermissionType { get; set; }
        public Permission Permission { get; set; }
        public string Description { get; set; }
    }

    public class PermissionItems
    {
        public Permission Permission { get; set; }
        public string Description { get; set; }
    }

    public class GroupPermission
    {
        public PermissionType PermissionType { get; set; }
        List<PermissionItems> permissionEnums { get; set; }
    }

    public class UserRolePermissionDTO
    {
        public int UserRolePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
}
