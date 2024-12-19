using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class ServiceEntity : EntityBase
    {
        [Key]
        public int ServiceId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string SignatureKey { get; set; }

        public virtual ICollection<UserServiceEntity> UserServices { get; set; }
    }
}
