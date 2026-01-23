using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class UserRoleEntity : EntityBase
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual RoleEntity Roles { get; set; }
        public virtual UserEntity Users { get; set; }
    }
}
