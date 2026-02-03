using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Helpers;
using IdentityCore.EFs.Requests;
using IdentityCore.Repository;
using IdentityCore.Repository.Interfaces;
using IdentityCore.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace IdentityCore.Business
{
    public class UserBusiness: BaseBusiness, IUserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        private readonly IEmailBusiness _emailBusiness;

        private readonly IUserRepository _userRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserServiceRepository _userServiceRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public UserBusiness(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IDistributedCache distributedCache,
            IEmailBusiness emailBusiness, 
            IUserRepository userRepository, 
            IUserPermissionRepository userPermissionRepository,
            IUserRoleRepository userRoleRepository,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IServiceRepository serviceRepository,
            IUserServiceRepository userServiceRepository,
            IRolePermissionRepository rolePermissionRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _distributedCache = distributedCache;

            _emailBusiness = emailBusiness;

            _userRepository = userRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _serviceRepository = serviceRepository;
            _userServiceRepository = userServiceRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<List<UserDTO>> GetAllAsync(Expression<Func<UserEntity, bool>> predicate = null)
        {
            var userEntities = await _userRepository.Get(predicate).ToListAsync();
            return _mapper.Map<List<UserDTO>>(userEntities);    
        }

        public async Task<UserDTO> GetSingleUserWithPermissionAndRoleAsync(string userName, List<string> appKeys, Guid? guid = null, string refreshToken = "")
        {
            var userQuery = _userRepository.Get();

            if(!string.IsNullOrEmpty(userName))
            {
                userQuery = userQuery.Where(s => (s.Email == userName || s.UserName == userName) && !s.IsDeleted);
            }
            else if(guid.HasValue)
            {
                userQuery = userQuery.Where(s => s.GUID == guid && s.IsLogin && !s.IsDeleted);
            }
            else
            {
                userQuery = userQuery.Where(s => s.RefreshToken == refreshToken && s.IsLogin &&!s.IsDeleted);
            }

            userQuery = userQuery.Include(s => s.UserPermissions)
                                .Include(s => s.UserRoles)
                                .Include(s => s.UserServices);

            var userEntity = await userQuery.FirstOrDefaultAsync();

            var userDto = _mapper.Map<UserDTO>(userEntity);

            if (userEntity != null)
            {
                if (userEntity.UserPermissions.Count > 0)
                {
                    var permissionIds = userEntity.UserPermissions.Select(s => s.PermissionId).ToList();
                    var permissionEntity = await _permissionRepository.Get(s => permissionIds.Contains(s.PermissionId) && !s.IsDeleted).Select(s => s.Name).ToListAsync();

                    userDto.Permissions = await _userPermissionRepository
                                                .Get(s => s.UserId == userDto.UserId && !s.IsDeleted)
                                                .Include(s => s.Permissions)
                                                .Select(s=> s.Permissions.Name)
                                                .ToListAsync();
                }

                if (userEntity.UserServices.Count > 0)
                {
                    userDto.Services = await _userServiceRepository
                                            .Get(s => s.UserId == userEntity.UserId && !s.IsDeleted)
                                            .Include(s => s.Services)
                                            .Where(s => appKeys.Any(x => x == s.Services.SignatureKey) &&
                                                        (!s.DateActive.HasValue && !s.DateExpired.HasValue) ||
                                                        (s.DateActive.HasValue && s.DateActive <= DateTime.UtcNow) &&
                                                        (s.DateExpired.HasValue && s.DateExpired > DateTime.UtcNow))
                                            .Select(s => new AssigningService()
                                            {
                                                GUID = s.Services.GUID,
                                                Key = s.Services.SignatureKey,
                                                Name = s.Services.Name,
                                                DateActive = s.DateActive,
                                                DateExpired = s.DateExpired,
                                            }).ToListAsync();
                }

                if (userEntity.UserRoles.Count > 0) 
                {
                    userDto.Roles = await _userRoleRepository
                            .Get(s => s.UserId == userDto.UserId && !s.IsDeleted)
                            .Include(s => s.Roles)
                            .ThenInclude(s => s.RolePermissions)
                            .ThenInclude(s => s.Permissions)
                            .Where(s => !s.Roles.IsLock)
                            .Select(s => new RoleResponse
                            {
                                Role = s.Roles.Name,
                                Permissions = s.Roles.RolePermissions
                                                        .Where(r => !r.Permissions.IsDeleted)
                                                        .Select(r => r.Permissions.Name)
                                                        .ToList()
                            }).ToListAsync();
                }
            }

            return userDto;
        }

        public async Task<PaginationItems<UserDTO>> GetUsersAsync(UserFetchRequest request)
        {
            var userQuery = _userRepository.Get(s => !s.IsDeleted);
            int pageNum = 1;
            int pageSize = 30;
            int totalCount = userQuery.Count(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.PageNum) && !string.IsNullOrEmpty(request.PageSize))
            {
                pageNum = int.Parse(request.PageNum);
                pageSize = int.Parse(request.PageSize);
            }

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

            userQuery = userQuery.Skip((pageNum - 1) * pageSize).Take(pageSize);

            var userEntities = await userQuery.ToListAsync();

            return new PaginationItems<UserDTO>(pageSize, pageNum, totalCount, _mapper.Map<List<UserDTO>>(userEntities));
        }

        public async Task<UserDTO> GetUserById(Guid guid)
        {
            var result = await _userRepository.Get(s => s.GUID == guid).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new FriendlyException(StatusCodes.Status204NoContent, "User not existed");
            }

            var userDto = _mapper.Map<UserDTO>(result);


            userDto.Roles = await _userRoleRepository
                            .Get(s => s.UserId == userDto.UserId && !s.IsDeleted)
                            .Include(s => s.Roles)
                            .ThenInclude(s => s.RolePermissions)
                            .ThenInclude(s => s.Permissions)
                            .Where(s => !s.Roles.IsLock)
                            .Select(s => new RoleResponse
                            {
                                Role = s.Roles.Name,
                                GUID = s.Roles.GUID,
                                Permissions = s.Roles.RolePermissions
                                                        .Where(r => !r.Permissions.IsDeleted)
                                                        .Select(r => r.Permissions.Name)
                                                        .ToList()
                            }).ToListAsync();

            userDto.Permissions = await _userPermissionRepository
                                               .Get(s => s.UserId == userDto.UserId && !s.IsDeleted)
                                               .Include(s => s.Permissions)
                                               .Select(s => s.Permissions.GUID.ToString())
                                               .ToListAsync();

            userDto.Services = await _userServiceRepository
                                             .Get(s => s.UserId == userDto.UserId && !s.IsDeleted)
                                             .Include(s => s.Services)
                                             .Select(s => new AssigningService()
                                             {
                                                 GUID = s.Services.GUID,
                                                 Name = s.Services.Name,
                                                 Key = s.Services.SignatureKey,
                                                 DateActive = s.DateActive,
                                                 DateExpired = s.DateExpired,
                                             })
                                             .ToListAsync();

            return userDto;

        }

        public async Task<UserDTO> CreateUserAsync(CreateOrUpdateUserRequest createUserRequest)
        {
            var isExistedUser = await _userRepository.Get(s => s.Email == createUserRequest.Email
                                                    || s.UserName == createUserRequest.UserName).AnyAsync();
            if (isExistedUser) 
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "User already existed");
            }

            var userDto = _mapper.Map<UserDTO>(createUserRequest);

            userDto.Step = (int)Step.WaitingConfirmed;
            userDto.OTPCode = _emailBusiness.GenerateOTP(GlobalConst.OTP.SizeCode);
            userDto.OTPLifeTime = DateTime.UtcNow.AddMinutes(GlobalConst.OTP.LifeTimeMinute);
            userDto.Password = EnscryptHelper.ConvertSHA256(userDto.Password ?? "");

            var userEntity = _mapper.Map<UserEntity>(userDto);

            var userCreated = _userRepository.Add(userEntity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task<bool> DeleteUserAsync(Guid guid)
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

        public async Task<UserDTO> UpdateUserAsync(UserDTO userDto, bool isRelatedToken = false,bool isLock = false)
        {
            var userEntity = await _userRepository
                                        .Get(s => (s.GUID == userDto.GUID || s.Email == userDto.Email) && !s.IsDeleted)
                                        .Include(s => s.UserPermissions)
                                        .ThenInclude(r => r.Permissions)
                                        .FirstOrDefaultAsync();

            if (userEntity == null) 
            {
                throw new FriendlyException(StatusCodes.Status404NotFound, "Not found user");
            }

            if (!string.IsNullOrEmpty(userDto.AvatarUrl))
            {
                userEntity.AvatarUrl = userDto.AvatarUrl;
            }

            if (!string.IsNullOrEmpty(userDto.FirstName))
            {
                userEntity.FirstName = userDto.FirstName;
            }

            if (!string.IsNullOrEmpty(userDto.MiddleName))
            {
                userEntity.MiddleName = userDto.MiddleName;
            }

            if (!string.IsNullOrEmpty(userDto.LastName))
            {
                userEntity.LastName = userDto.LastName;
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

            if (userDto.AttemptLogin.HasValue) 
            {
                userEntity.AttemptLogin = (int)userDto.AttemptLogin;
            }

            if (userDto.ExpirationDate.HasValue)
            {
                userEntity.ExpirationDate = userDto.ExpirationDate;
            }

            if (userDto.IsRequiredChangePassword.HasValue)
            {
                if(userDto.Roles != null && userDto.Roles.Any(s => s.Value >= (int)Role.Administrator))
                {
                    throw new FriendlyException(StatusCodes.Status400BadRequest, "User is not have permission");
                }

                userEntity.IsRequiredChangePassword = userDto.IsRequiredChangePassword.Value;
            }

            if (userDto.IsLogin.HasValue) 
            {
                userEntity.IsLogin = (bool)userDto.IsLogin;

                if ((bool)userDto.IsLogin)
                {
                    userEntity.LastLogin = DateTime.UtcNow;
                }
            }

            if (userDto.IsActive.HasValue)
            {
                userEntity.IsActive = (bool)userDto.IsActive;
            }

            if (userDto.OTPCode != "")
            {
                userEntity.OTPCode = userDto.OTPCode;
                userEntity.OTPLifeTime = userDto.OTPLifeTime;
            }

            if(userDto.Step.HasValue)
            {
                userEntity.Step = (int)userDto.Step;
            }

            if (isRelatedToken)
            {
                userEntity.RefreshToken = userDto.RefreshToken;
            }

            /* [Caution] Props must have change after update */
            userEntity.Locked = userDto.Locked;
            userEntity.FcmToken = userDto.FcmToken;

            _userRepository.Update(userEntity);
            await _unitOfWork.CommitAsync();

            if (isLock)
            {
                if (userEntity.Locked != null)
                {
                    var sendMailLock = await _distributedCache.GetStringAsync($"{KeyCache.Flags}:SentMail_Lock:{userEntity.GUID}");

                    if (string.IsNullOrEmpty(sendMailLock))
                    {
                        var payload = new { IsSent = true };
                        await _distributedCache.SetStringAsync(
                                   $"{KeyCache.Flags}:SentMail_Lock:{userEntity.GUID}",
                                   JsonConvert.SerializeObject(payload),
                                   new DistributedCacheEntryOptions
                                   {
                                       AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                                   }
                               );

                        await _emailBusiness.SendMailAsync(_mapper.Map<UserDTO>(userEntity), TemplateEmailType.Locked);
                    }
                }
            }

            

            return _mapper.Map<UserDTO>(userEntity);
        }
    }
}
