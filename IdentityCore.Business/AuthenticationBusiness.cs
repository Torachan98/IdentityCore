using IdentityCore.Business.Interfaces;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityCore.EFs.Requests;
using AutoMapper;
using IdentityCore.Repository.Interfaces;
using IdentityCore.EFs.Helpers;
using Microsoft.EntityFrameworkCore;
using IdentityCore.Repository.UnitOfWork;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using IdentityCore.EFs.Enums;

namespace IdentityCore.Business
{
    public class AuthenticationBusiness : BaseBusiness, IAuthenticationBusiness
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;

        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IUserRepository _userRepository;
        
        public AuthenticationBusiness(IMapper mapper, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IUserBusiness userBusiness, IEmailBusiness emailBusiness, IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
            _userRepository = userRepository;
        }

        public async Task<ObjectResponse<AuthenticationToken>> SignInAsync(SignInRequest signInRequest)
        {
            var user = await _userBusiness.GetSingleUserWithPermissionAndRoleAsync(signInRequest.UserName);
            var passwordEnscrypt = EnscryptHelper.ConvertSHA256(signInRequest.Password);

            if(user == null)
            {
                return new ObjectResponse<AuthenticationToken>()
                {
                    Message = "User or password is not corrected"
                };
            }

            if (!Equals(user.Password, passwordEnscrypt))
            {
                if(user.AttemptLogin == GlobalConfiguration.AccountLocked.AttemptNumber)
                {
                    user.Locked = DateTime.UtcNow.AddHours(GlobalConfiguration.AccountLocked.LockedHour);
                    await _userBusiness.UpdateUserAsync(user);

                    return new ObjectResponse<AuthenticationToken>()
                    {
                        Message = "This user has been locked"
                    };
                }

                user.AttemptLogin = user.AttemptLogin + 1;

                await _userBusiness.UpdateUserAsync(user);

                return new ObjectResponse<AuthenticationToken>()
                {
                    Message = "User or password is not corrected"
                };
            }

            if(user.Locked > DateTime.UtcNow)
            {
                return new ObjectResponse<AuthenticationToken>()
                {
                    Message = "This user has been locked"
                };
            }

            if (user.IsActive.HasValue && !(bool)user.IsActive)
            {
                user.OTPCode = _emailBusiness.GenerateOTP(GlobalConfiguration.OTP.SizeCode);
                user.OTPLifeTime = DateTime.UtcNow.AddMinutes(GlobalConfiguration.OTP.LifeTimeMinute);

                await _emailBusiness.SendMailAsync(user, TemplateEmailType.OTP);
                await _userBusiness.UpdateUserAsync(user);

                return new ObjectResponse<AuthenticationToken>()
                {
                    Message = "User is not active"
                };
            }

            if (user.IsLogin != null && (bool)user.IsLogin)
            {
                return new ObjectResponse<AuthenticationToken>()
                {
                    Message = "User already login"
                };
            }

            var result = new AuthenticationToken()
            {
                RefreshToken = GenerateRefreshToken(),
                AccessToken = GenerateToken(user)
            };

            user.IsLogin = true;
            user.RefreshToken = result.RefreshToken;

            await _userBusiness.UpdateUserAsync(user);
            await _distributedCache.SetStringAsync($"{KeyCache.User}-{user.GUID}", JsonSerializer.Serialize(user));

            return new ObjectResponse<AuthenticationToken>()
            {
                Item = result
            };
        }

        public async Task<bool> SignOutAsync(UserDTO user, string accessToken)
        {
            user.IsLogin = false;
            user.RefreshToken = "";

            var userDto = await _userBusiness.UpdateUserAsync(user);
            if(userDto == null)
            {
                return false;
            }

            var tokenBlacklist = await _distributedCache.GetStringAsync(KeyCache.BlackList) ?? "";
            var blacklist = !string.IsNullOrEmpty(tokenBlacklist) ? JsonSerializer.Deserialize<List<string>>(tokenBlacklist) : new List<string>();
            blacklist.Add(accessToken);
            await _distributedCache.SetStringAsync(KeyCache.BlackList, JsonSerializer.Serialize(blacklist));
            await _distributedCache.RemoveAsync($"{KeyCache.User}-{user.GUID}");
            return true;
        }

        public async Task<AuthenticationToken> RenewTokenAsync(string refreshToken)
        {
            var userDto = await _userBusiness.GetSingleUserWithPermissionAndRoleAsync("","",refreshToken);

            if (userDto.Locked.HasValue && userDto.Locked >= DateTime.UtcNow)
            {
                throw new FriendlyException(StatusCodes.Status401Unauthorized, "User has been locked");
            }

            if (userDto == null) 
            {
                return null;
            }

            return new AuthenticationToken()
            {
                RefreshToken = userDto.RefreshToken,
                AccessToken = GenerateToken(userDto)
            };
        }

        private string GenerateToken(UserDTO user)
        {
            var permissionItem = user.GroupPermissions.GroupBy(s => s.PermissionType).Select(s => new
            {
                PermissionGroup = s.Key.ToString(),
                Permssions = s.Select(i => new
                {
                    Permission = i.Permission.ToString(),
                    Type = i.PermissionType.ToString(),
                    Description = i.Description,
                }).ToList()
            }).ToList();

            var claims = new[] {
                new Claim("name",user.FullName),
                new Claim("email",user.Email),
                new Claim("phone",$"{user.PhoneCode} {user.Phone}"),
                new Claim("userId",user.GUID),
                new Claim("permissions",JsonSerializer.Serialize(permissionItem)),
                new Claim("services",JsonSerializer.Serialize(user.Services)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalConfiguration.Jwt.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(GlobalConfiguration.Jwt.Issuer,
                                              GlobalConfiguration.Jwt.Audience,
                                              claims,
                                              expires: DateTime.UtcNow.AddMinutes(GlobalConfiguration.Jwt.LifeTime),
                                              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public Task<bool> ValidateToken()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmOTPAsync(string otpCode)
        {
            if (string.IsNullOrEmpty(otpCode))
            {
                return false;
            }

            var userEntity = await _userRepository.Get(s=> s.OTPCode == otpCode && !s.IsActive && !s.IsDeleted).FirstOrDefaultAsync();

            if (userEntity == null) 
            {
                return false;
            }

            if(userEntity.OTPLifeTime > DateTime.UtcNow)
            {
                await _userBusiness.UpdateUserAsync(new UserDTO() 
                {
                    GUID = userEntity.GUID,
                    IsActive = true
                });

                return true;
            }

            return false;
        }

        public async Task<bool> ResetEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var userEntity = await _userRepository.Get().FirstOrDefaultAsync(s => s.Email == email && s.IsActive);
            if (userEntity == null) 
            {
                return false;
            }

            userEntity.OTPCode = _emailBusiness.GenerateOTP(5);
            userEntity.OTPLifeTime = DateTime.UtcNow.AddMinutes(15);
            _userRepository.Add(userEntity);

            var userDto = _mapper.Map<UserDTO>(userEntity);            
            await _emailBusiness.SendMailAsync(userDto, TemplateEmailType.OTP);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> ResetEmailConfirmAsync(string email, string otpCode)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var userEntity = await _userRepository.Get().FirstOrDefaultAsync(s => s.Email == email && 
                                                                            s.OTPCode == otpCode &&
                                                                            s.OTPLifeTime.HasValue && s.OTPLifeTime >= DateTime.UtcNow && 
                                                                            s.IsActive);
            if (userEntity == null)
            {
                return false;
            }

            userEntity.Email = email;
            userEntity.OTPLifeTime = DateTime.UtcNow;
            _userRepository.Update(userEntity, s => s.Email);

            await _unitOfWork.CommitAsync();
            return true;
        }

        public Task<bool> ResetPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetPasswordConfirmAsync(string password, string otpCode)
        {
            throw new NotImplementedException();
        }
    }
}
