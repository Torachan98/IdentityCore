using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class ServiceEntity : EntityBase
    {
        [Key]
        public int ServiceId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public string SignatureKey { get; set; }

        public virtual ICollection<UserServiceEntity> UserServices { get; set; }
    }
}
