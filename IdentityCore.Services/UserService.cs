using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Helpers;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IdentityCore.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;

        public UserService(IMapper mapper,IUserBusiness userBusiness, IEmailBusiness emailBusiness)
        {
            _mapper = mapper;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
        }

        public async Task<PaginationItems<UserDTO>> GetAllAsync(UserFetchRequest request)
        {
            return await _userBusiness.GetUsersAsync(request);
        }

        public async Task<UserDTO> GetByIdAsync(string guid)
        {
            return await _userBusiness.GetUserById(guid);
        }

        public async Task<UserDTO> CreateAsync(CreateOrUpdateUserRequest request)
        {
            var errors = Validator.ValidateRequiredProperties(request);
            if (errors.Any())
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest,JsonConvert.SerializeObject(errors));
            }

            var result = await _userBusiness.CreateUserAsync(request);
            await _emailBusiness.SendMailAsync(result, TemplateEmailType.OTP);

            return result;
        }

        public async Task<UserDTO> UpdateAsync(CreateOrUpdateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.GUID))
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Id user do not allow empty");
            }

            var userDto = _mapper.Map<UserDTO>(request);

            return await _userBusiness.UpdateUserAsync(userDto);
        }

        public async Task<bool> DeleteAsync(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return false;
            }

            return await _userBusiness.DeleteUserAsync(guid);
        }

        public Task<UserDTO> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest)
        {
            throw new NotImplementedException();
        }
    }
}
