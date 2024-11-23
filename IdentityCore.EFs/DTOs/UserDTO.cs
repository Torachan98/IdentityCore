using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Enums;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace IdentityCore.EFs.DTOs
{
    public class UserDTO
    {
        public string AvatarUrl { get; set; } 
        public string UserName { get; set; } 
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public string Password { get; set; } 
        public string Phone { get; set; } 
        public string PhoneCode { get; set; } 
        public string Region { get; set; } 
        public string GUID { get; set; } 
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [IgnoreDataMember]
        public bool? IsRequiredChangePassword { get; set; }

        [IgnoreDataMember]
        public bool IsPasswordExpired { get; set; }

        [IgnoreDataMember]
        public bool IsDeleted { get; set; }   
        
        [IgnoreDataMember]
        public bool? IsLogin { get; set; }

        [IgnoreDataMember]
        public bool? IsActive { get; set; }

        [IgnoreDataMember]
        public string OTPCode {get;set;} 

        [IgnoreDataMember]
        public int? AttemptLogin { get; set; }

        [IgnoreDataMember]
        public DateTime? Locked { get; set; }

        [IgnoreDataMember]
        public DateTime? LastLogin { get; set; }

        [IgnoreDataMember]
        public DateTime? ExpirationDate { get; set; }

        [IgnoreDataMember]
        public DateTime? OTPLifeTime { get; set; }

        [IgnoreDataMember]
        public string RefreshToken { get; set; } 

        public List<PermissionEnum> GroupPermissions { get; set; }
        public List<RoleEnum> GroupRoles { get; set; }

        public List<UserRolePermissionEntity> UserRolePermissions { get; set; }
    }
}
