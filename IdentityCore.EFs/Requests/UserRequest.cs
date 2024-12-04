using IdentityCore.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace IdentityCore.EFs.Requests
{
    public class UserRequest
    {
        public string AvatarUrl { get; set; } 
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; } 
        public string PhoneCode { get; set; } 
        public string Region { get; set; }

        [AllowNotValidate]
        [DefaultValue(false)]
        public bool ChangedPasswordFirstTime { get; set; }

        [AllowNotValidate]
        public string RefreshToken { get; set; }

        [IgnoreDataMember]
        [AllowNotValidate]
        public bool? IsLogin { get; set; }
    }

    public class CreateOrUpdateUserRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
        public string Region { get; set; }
    }

    public class SignInRequest
    {
        public string UserName { get; set; } 
        public string Password { get; set; } 
    }

    public class UserFetchRequest
    {
        public string Keyword { get; set; }
        public List<string> Regions { get; set; }
        public string PageSize { get; set; } 
        public string PageNum { get; set; } 
    }

    public class ForgotPasswordRequest
    {
        [Required]
        public string Password { get; set; } 

        [Required]
        public string Email { get; set; } 
        public string OTPCode { get; set; } 
    }
}
