using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Helpers;
using IdentityCore.Services.Interfaces;
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

        public Task<UserDTO> GetByIdAsync(string guid)
        {
            throw new NotImplementedException();
        }

        public async Task<ObjectResult<UserDTO>> CreateAsync(CreateOrUpdateUserRequest request)
        {
            var errors = Validator.ValidateRequiredProperties(request);
            if (errors.Any())
            {
                return new ObjectResult<UserDTO>()
                {
                    Message = JsonConvert.SerializeObject(errors)
                };
            }

            var result = await _userBusiness.CreateUserAsync(request);

            if (result == null)
            {
                return new ObjectResult<UserDTO>()
                {
                    Message = "User has been existed or has been inactive"
                };
            }

            await _emailBusiness.SendMailAsync(result, TemplateEmailType.OTP);

            return new ObjectResult<UserDTO>()
            {
                Item = result,
            };
        }

        public async Task<ObjectResult<UserDTO>> UpdateAsync(CreateOrUpdateUserRequest request)
        {
            var errors = Validator.ValidateRequiredProperties(request);
            if (errors.Any())
            {
                return new ObjectResult<UserDTO>()
                {
                    Message = JsonConvert.SerializeObject(errors)
                };
            }

            var userDto = _mapper.Map<UserDTO>(request);

            var result = await _userBusiness.UpdateUserAsync(userDto);

            return new ObjectResult<UserDTO>()
            {
                Item = result,
            };
        }

        public async Task<bool> DeleteAsync(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return false;
            }

            return await _userBusiness.DeleteUserAsync(guid);
        }

        public Task<ObjectResult<UserDTO>> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest)
        {
            throw new NotImplementedException();
        }
    }
}
