using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Services.Interfaces
{
    public interface IUserService: IBaseService<UserDTO, CreateOrUpdateUserRequest, UserFetchRequest>
    {
        Task<UserDTO> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
    }
}
