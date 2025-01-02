using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.EFs.DTOs
{
    public class AuthenticationDTO
    {

    }

    public class SignInRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public List<string> AppKeys { get; set; } = new List<string>();
    }

    public class ForgotPasswordRequest
    {
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Email { get; set; }
        public string? OTPCode { get; set; }
    }

    public class OTPRequest
    {
        public string? OTPCode { get; set; }
    }
}
