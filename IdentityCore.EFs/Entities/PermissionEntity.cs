using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class PermissionEntity : EntityBase
    {
        [Key]
        public int PermissionId {get;set;}

        [Required]
        public PermissionType PermissionType { get;set;}

        [Required]
        public string Name { get; set; } 
        public string Description { get; set; }

        public virtual UserRolePermissionEntity UserRolePermissions{ get; set; }
    }
}
