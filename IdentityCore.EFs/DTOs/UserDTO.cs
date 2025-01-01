using IdentityCore.Attributes;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Requests;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IdentityCore.EFs.DTOs
{
    public class UserDTO
    {
        [IgnoreDataMember]
        public int UserId { get; set; }
        public string? AvatarUrl { get; set; } 
        public string? UserName { get; set; } 
        public string? FullName { get; set; } 
        public string? Email { get; set; }

        [IgnoreDataMember]
        public string? Password { get; set; } 
        public string? Phone { get; set; } 
        public string? PhoneCode { get; set; } 
        public string? Region { get; set; } 
        public string? GUID { get; set; } 
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
        public string? OTPCode {get;set;} 

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
        public string? RefreshToken { get; set; } 

        public List<PermissionEnum> GroupPermissions { get; set; } = new List<PermissionEnum>();
        public List<RoleEnum> GroupRoles { get; set; } = new List<RoleEnum>();
        public List<string> Services { get; set; } = new List<string>();
        public List<RolePermissionEntity> RolePermissions { get; set; } = new List<RolePermissionEntity>();
    }

    public class UserFetchRequest: FetchParams
    {
        public List<string> Regions { get; set; } = new List<string>();
    }

    public class CreateOrUpdateUserRequest
    {
        public string? GUID { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? PhoneCode { get; set; }
        public string? Region { get; set; }
    }

    public class UserRequest
    {
        public string? AvatarUrl { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? PhoneCode { get; set; }
        public string? Region { get; set; }

        [AllowNotValidate]
        [DefaultValue(false)]
        public bool ChangedPasswordFirstTime { get; set; }

        [AllowNotValidate]
        public string? RefreshToken { get; set; }

        [IgnoreDataMember]
        [AllowNotValidate]
        public bool? IsLogin { get; set; }
    }
}
