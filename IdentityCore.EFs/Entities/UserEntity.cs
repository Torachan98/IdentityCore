using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class UserEntity: EntityBase
    {
        [Key]
        public int UserId { get; set; }
        public string AvatarUrl { get; set; } 

        [Required]
        public string UserName { get; set; } 

        [Required]
        public string FirstName { get; set; } 

        [Required]
        public string LastName { get; set; } 

        [Required]
        public string MiddleName { get; set; } 

        [Required]
        public string Email { get; set; } 

        [Required]
        public string Password { get; set; } 

        [Required]
        public string Phone { get; set; } 

        [Required]
        public string PhoneCode { get; set; } 

        [Required]
        public string Region { get; set; } 

        [DefaultValue(false)]
        public bool IsRequiredChangePassword { get; set; }

        [DefaultValue(false)]
        public bool IsPasswordExpired { get; set; }    
        
        [DefaultValue(false)]
        public bool IsLogin { get; set; }  
        
        [DefaultValue(false)]
        public bool IsActive { get; set; }

        public int AttemptLogin { get; set; }
        public DateTime? Locked { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? OTPLifeTime { get; set; }
        public string OTPCode { get; set; } 
        public string RefreshToken { get; set; } 

        public virtual ICollection<UserRolePermissionEntity> UserRolePermissions { get; set; }
    }
}
