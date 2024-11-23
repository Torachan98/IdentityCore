using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginationItems<UserDTO>> GetUsersAsync(UserFetchRequest userFetchRequest);
        Task<ObjectResult<UserDTO>> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<ObjectResult<UserDTO>> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
        Task<ObjectResult<UserDTO>> UpdateUserAsync(UserRequest userRequest);
        Task<bool> DeleteUserAsync(string guid);
    }
}
