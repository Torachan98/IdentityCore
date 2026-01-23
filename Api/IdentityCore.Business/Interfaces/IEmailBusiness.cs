using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Business.Interfaces
{
    public interface IEmailBusiness
    {
        Task<bool> SendMailAsync(UserDTO user, TemplateEmailType templateType);

        string GenerateOTP(int sizeCode);
    }
}
