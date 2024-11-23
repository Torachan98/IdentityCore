using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class UserRolePermissionEntity: EntityBase
    {
        [Key]
        public int UserRolePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual UserEntity Users { get; set; }
        public virtual RoleEntity Roles { get; set; }
        public virtual PermissionEntity Permissions { get; set; }
    }
}
