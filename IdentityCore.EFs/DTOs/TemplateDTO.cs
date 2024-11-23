using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.EFs.DTOs
{
    public class TemplateDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class TemplateOTPDTO
    {
        public string Name { get; set; }   
        public string OTP { get; set; }
    }

    public class TemplateForgotPasswordDTO
    {
    }
}
