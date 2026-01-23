using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class PermissionEntity : EntityBase
    {
        [Key]
        public int PermissionId {get;set;}
        public required string Name { get; set; } 
        public int Value { get; set; } 
        public string? Description { get; set; }

        public virtual ICollection<RolePermissionEntity> RolePermissions{ get; set; }
        public virtual ICollection<UserPermissionEntity> UserPermissions{ get; set; }
    }
}
