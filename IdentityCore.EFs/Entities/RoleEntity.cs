using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class RoleEntity: EntityBase
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string? RoleName { get; set; } 
        public string? Description { get; set; } 

        [DefaultValue(false)]
        public bool IsLock { get; set; }

        public virtual ICollection<UserRolePermissionEntity> UserRolePermissions { get; set; }
    }
}
