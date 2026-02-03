using AutoMapper;
using Google.Apis.Drive.v3.Data;
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
        private readonly IRoleBusiness _roleBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IPermissionBusiness _permissionBusiness;
        private readonly IServiceBusiness _serviceBusiness;

        public UserService(IMapper mapper,
            IUserBusiness userBusiness, 
            IEmailBusiness emailBusiness, 
            IRoleBusiness roleBusiness, 
            IPermissionBusiness permissionBusiness,
            IServiceBusiness serviceBusiness)
        {
            _mapper = mapper;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
            _roleBusiness = roleBusiness;
            _permissionBusiness = permissionBusiness;
            _serviceBusiness = serviceBusiness;
        }

        public async Task<PaginationItems<UserDTO>> GetAllAsync(UserFetchRequest request)
        {
            return await _userBusiness.GetUsersAsync(request);
        }

        public async Task<UserDTO> GetByIdAsync(Guid guid)
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
            if (!request.GUID.HasValue)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Id user do not allow empty");
            }

            var userDto = _mapper.Map<UserDTO>(request);
            var userUpdated = await _userBusiness.UpdateUserAsync(userDto, isLock: request.IsLock);

            if(request.Roles.Count > 0)
            {
                var roleIds = request.Roles
                    .Where(r => r.GUID.HasValue)
                    .Select(s => s.GUID)
                    .OfType<Guid>().
                    ToList();

                var roleDtos = await _roleBusiness.AssignUserRolesAsync(roleIds, userUpdated.UserId);

                userUpdated.Roles = roleDtos.Select(s => new RoleResponse()
                {
                    GUID = s.GUID,
                    Role = s.Name,
                    Value = (int)s.Role,
                    Permissions = new List<string>()
                }).ToList();
            }

            if (request.Permissions.Count > 0) 
            {
                var permissionDtos = await _permissionBusiness.AssignUserPermissionsAsync(request.Permissions.Select(s => Guid.Parse(s)).ToList(), userUpdated.UserId);
                userUpdated.Permissions = permissionDtos.Select(s => s.Guid.ToString().ToUpper()).ToList();
            }

            if (request.Services.Count > 0) 
            {
                var assigningServices = await _serviceBusiness.AssigningUserServiceAsync(request.Services, userUpdated.UserId);
                userUpdated.Services = assigningServices ?? new List<AssigningService>();
            }

            return userUpdated;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            return await _userBusiness.DeleteUserAsync(guid);
        }

        public Task<UserDTO> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest)
        {
            throw new NotImplementedException();
        }
    }
}
