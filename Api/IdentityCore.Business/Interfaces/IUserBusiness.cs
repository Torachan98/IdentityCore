using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Requests;
using System.Linq.Expressions;

namespace IdentityCore.Business.Interfaces
{
    public interface IUserBusiness
    {
        /// <summary>
        /// Choosing one of 3 options to get data: UserName or GUID or Refresh Token,
        /// with Username method are not need for checking status login otherwise must need
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="guid"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<UserDTO> GetSingleUserWithPermissionAndRoleAsync(string userName, List<string> appKeys, Guid? guid = null, string refreshToken = "");
        Task<UserDTO> GetUserById(Guid guid);
        Task<List<UserDTO>> GetAllAsync(Expression<Func<UserEntity, bool>> predicate = null);
        Task<PaginationItems<UserDTO>> GetUsersAsync(UserFetchRequest userFetchRequest);
        Task<UserDTO> CreateUserAsync(CreateOrUpdateUserRequest createUserRequest);
        Task<UserDTO> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
        Task<UserDTO> UpdateUserAsync(UserDTO userDTO, bool isRelatedToken = false, bool isLock =  false);
        Task<bool> DeleteUserAsync(Guid guid);
    }
}
