using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Helpers;
using IdentityCore.EFs.Requests;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IdentityCore.Business
{
    public class UserBusiness: BaseBusiness, IUserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IEmailBusiness _emailBusiness;

        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserServiceRepository _userServiceRepository;

        public UserBusiness(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IEmailBusiness emailBusiness, 
            IUserRepository userRepository, 
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IServiceRepository serviceRepository,
            IUserServiceRepository userServiceRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _emailBusiness = emailBusiness;

            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _serviceRepository = serviceRepository;
            _userServiceRepository = userServiceRepository;
        }

        public async Task<UserDTO> GetSingleUserWithPermissionAndRoleAsync(string userName, string guid = "",string refreshToken = "")
        {
            var userQuery = _userRepository.Get();

            if(!string.IsNullOrEmpty(userName))
            {
                userQuery = userQuery.Where(s => (s.Email == userName || s.UserName == userName) && !s.IsDeleted);
            }
            else if(!string.IsNullOrEmpty(guid))
            {
                userQuery = userQuery.Where(s => s.GUID == guid && s.IsLogin && !s.IsDeleted);
            }
            else
            {
                userQuery = userQuery.Where(s => s.RefreshToken == refreshToken && s.IsLogin &&!s.IsDeleted);
            }

            userQuery = userQuery.Include(s => s.UserRolePermissions);

            var userEntity = await userQuery.FirstOrDefaultAsync();

            var userDto = _mapper.Map<UserDTO>(userEntity);

            if (userEntity != null)
            {
                if (userEntity.UserRolePermissions != null)
                {
                    var permissionIds = userEntity.UserRolePermissions.Select(s => s.PermissionId).ToList();
                    var roleIds = userEntity.UserRolePermissions.Select(s => s.RoleId).ToList();

                    var permissionEntity = await _permissionRepository.Get(s => permissionIds.Contains(s.PermissionId)).ToListAsync();
                    var roleEntity = await _roleRepository.Get(s => roleIds.Contains(s.RoleId)).ToListAsync();

                    userDto.Services = await _userServiceRepository
                                                .Get(s => s.UserId == userEntity.UserId)
                                                .Include(s => s.Services)
                                                .Select(s => s.Services.SignatureKey)
                                                .ToListAsync();

                    if (permissionEntity != null)
                    {
                        var permissions = EnumHelper.ConvertPermissionToList<Permission, PermissionEnum>();
                        userDto.GroupPermissions = permissions.Where(s => permissionEntity.Any(r => r.Name == s.Permission.ToString() && r.PermissionType == s.PermissionType)).ToList();
                    }

                    var roles = EnumHelper.ConvertRoleToList<Role, RoleEnum>();
                    userDto.GroupRoles = roles.Where(s => roleEntity.Any(r => r.RoleName == s.Role.ToString())).ToList();
                }
            }

            return userDto;
        }

        public async Task<PaginationItems<UserDTO>> GetUsersAsync(UserFetchRequest request)
        {
            var userQuery = _userRepository.Get(s => !s.IsDeleted);
            int pageNum = 0;
            int pageSize = 30;
            int totalCount = userQuery.Count(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.PageNum) && !string.IsNullOrEmpty(request.PageSize))
            {
                pageNum = int.Parse(request.PageNum);
                pageSize = int.Parse(request.PageSize);
            }

            userQuery = userQuery.Skip(pageNum * (pageSize - 1)).Take(pageSize);

            if (request.Regions.Any())
            {
                userQuery = userQuery.Where(s => request.Regions.Contains(s.Region));
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                userQuery = userQuery.Where(s => !string.IsNullOrEmpty(s.Phone) && s.Phone.Contains(request.Keyword) ||
                                                !string.IsNullOrEmpty(s.Email) && s.Email.Contains(request.Keyword) ||
                                                !string.IsNullOrEmpty(s.UserName) && s.UserName.Contains(request.Keyword) ||
                                                !string.IsNullOrEmpty(s.FirstName) && s.FirstName.Contains(request.Keyword) ||
                                                !string.IsNullOrEmpty(s.LastName) && s.LastName.Contains(request.Keyword) ||
                                                !string.IsNullOrEmpty(s.MiddleName) && s.MiddleName.Contains(request.Keyword));
            }

            var userEntities = await userQuery.ToListAsync();

            return new PaginationItems<UserDTO>(pageSize, pageNum, totalCount, _mapper.Map<List<UserDTO>>(userEntities));
        }

        public async Task<UserDTO> CreateUserAsync(CreateOrUpdateUserRequest createUserRequest)
        {
            var isExistedUser = await _userRepository.Get(s => s.Email == createUserRequest.Email
                                                    || s.UserName == createUserRequest.UserName).AnyAsync();
            if (isExistedUser) 
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "User already existed");
            }

            var userEntity = _mapper.Map<UserEntity>(createUserRequest);

            userEntity.OTPCode = _emailBusiness.GenerateOTP(GlobalConst.OTP.SizeCode);
            userEntity.OTPLifeTime = DateTime.UtcNow.AddMinutes(GlobalConst.OTP.LifeTimeMinute);

            var userCreated = _userRepository.Add(userEntity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UserDTO>(createUserRequest);
        }

        public async Task<bool> DeleteUserAsync(string guid)
        {
            var userEntity = await _userRepository.Get(s => s.GUID == guid && !s.IsDeleted).FirstOrDefaultAsync();

            if (userEntity != null) 
            {
                _userRepository.DeleteWhere(s => s.UserId == userEntity.UserId);
                await _unitOfWork.CommitAsync();
                return true;
            }

            return false;
        }

        public Task<UserDTO> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO userDto)
        {
            var userEntity = await _userRepository
                                        .Get(s => (s.GUID == userDto.GUID || s.Email == userDto.Email) && !s.IsDeleted)
                                        .Include(s => s.UserRolePermissions)
                                        .ThenInclude(r => r.Permissions)
                                        .FirstOrDefaultAsync();

            if (userEntity == null) 
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "Not found user");
            }

            if (!string.IsNullOrEmpty(userDto.AvatarUrl))
            {
                userEntity.AvatarUrl = userDto.AvatarUrl;
            }

            if (userDto.RefreshToken != null) 
            {
                userEntity.RefreshToken = userDto.RefreshToken;
            }

            if (!string.IsNullOrEmpty(userDto.Phone))
            {
                userEntity.Phone = userDto.Phone;
            }

            if (!string.IsNullOrEmpty(userDto.PhoneCode))
            {
                userEntity.PhoneCode = userDto.PhoneCode;
            }

            if (!string.IsNullOrEmpty(userDto.Region))
            {
                userEntity.Region = userDto.Region;
            }

            if (userDto.AttemptLogin != null) 
            {
                userEntity.AttemptLogin = (int)userDto.AttemptLogin;
            } 
            
            if (userDto.Locked != null) 
            {
                userEntity.Locked = (DateTime)userDto.Locked;
            }

            if (userDto.IsRequiredChangePassword != null)
            {
                if(userDto.GroupRoles != null && userDto.GroupRoles.Any(s => s.Role <= Role.Administrator))
                {
                    throw new FriendlyException(StatusCodes.Status400BadRequest, "User is not have permission");
                }

                userEntity.IsRequiredChangePassword = (bool)userDto.IsRequiredChangePassword;
            }

            if (userDto.IsLogin != null) 
            {
                userEntity.IsLogin = (bool)userDto.IsLogin;

                if ((bool)userDto.IsLogin)
                {
                    userEntity.LastLogin = DateTime.UtcNow;
                }
            }

            if (userDto.IsActive != null)
            {
                userEntity.IsActive = (bool)userDto.IsActive;
            }

            if (userDto.OTPCode != "")
            {
                userEntity.OTPCode = userDto.OTPCode;
                userEntity.OTPLifeTime = userDto.OTPLifeTime;
            }

            _userRepository.Update(userEntity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UserDTO>(userEntity);
        }
    }
}
