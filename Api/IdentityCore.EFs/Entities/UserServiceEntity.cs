using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class UserServiceEntity: EntityBase
    {
        [Key]
        public int UserServiceId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; } 
        public DateTimeOffset? DateActive { get; set; }
        public DateTimeOffset? DateExpired { get; set; }

        public virtual UserEntity Users { get; set; }
        public virtual ServiceEntity Services { get; set; }
    }
}
