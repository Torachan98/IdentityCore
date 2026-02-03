using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class UserEntity: EntityBase
    {
        [Key]
        public int UserId { get; set; }
        public string? AvatarUrl { get; set; } 
        public required string UserName { get; set; } 
        public required string FirstName { get; set; } 
        public required string LastName { get; set; } 
        public required string MiddleName { get; set; } 
        public required string Email { get; set; } 
        public required string Password { get; set; } 
        public required string Phone { get; set; } 
        public required string PhoneCode { get; set; } 
        public required string Region { get; set; }

        [DefaultValue(0)]
        public int Step { get; set; }

        [DefaultValue(false)]
        public bool IsRequiredChangePassword { get; set; }

        [DefaultValue(false)]
        public bool IsPasswordExpired { get; set; }    
        
        [DefaultValue(false)]
        public bool IsLogin { get; set; }  
        
        [DefaultValue(false)]
        public bool IsActive { get; set; }

        public int AttemptLogin { get; set; }
        public DateTimeOffset? Locked { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
        public DateTimeOffset? OTPLifeTime { get; set; }
        public string? OTPCode { get; set; } 
        public string? RefreshToken { get; set; }
        public string? FcmToken { get; set; }

        public virtual ICollection<UserServiceEntity> UserServices { get; set; }
        public virtual ICollection<UserPermissionEntity> UserPermissions { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
