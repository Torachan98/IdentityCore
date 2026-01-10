using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class RoleEntity: EntityBase
    {
        [Key]
        public int RoleId { get; set; }
        public required string Name { get; set; }
        public int Value { get; set; }
        public string? Description { get; set; } 

        [DefaultValue(false)]
        public bool IsLock { get; set; }

        public virtual ICollection<RolePermissionEntity> RolePermissions { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
