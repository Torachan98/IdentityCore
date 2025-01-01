using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class RolePermissionEntity: EntityBase
    {
        [Key]
        public int RolePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }

        public virtual RoleEntity Roles { get; set; }
        public virtual PermissionEntity Permissions { get; set; }
    }
}
