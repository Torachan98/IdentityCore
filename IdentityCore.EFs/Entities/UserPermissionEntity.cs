using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class UserPermissionEntity: EntityBase
    {
        [Key]
        public int UserPermissionId { get; set; }
        public int PermissionId { get; set; }
        public int UserId { get; set; }

        public virtual UserEntity Users { get; set; }
        public virtual PermissionEntity Permissions { get; set; }
    }
}
