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
using System.Text.Json;
using IdentityCore.Repository.Interfaces;
using IdentityCore.EFs.Helpers;
using Microsoft.EntityFrameworkCore;

namespace IdentityCore.Business
{
    public class AuthenticationBusiness : BaseBusiness, IAuthenticationBusiness
    {
        private readonly IMapper _mapper;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IUserRepository _userRepository;
        public AuthenticationBusiness(IMapper mapper, IUserBusiness userBusiness, IEmailBusiness emailBusiness, IUserRepository userRepository)
        {
            _mapper = mapper;
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

            if (user.IsActive != null && !(bool)user.IsActive)
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

            return new ObjectResponse<AuthenticationToken>()
            {
                Item = result
            };
        }

        public async Task<bool> SignOutAsync(UserDTO user)
        {
            user.IsLogin = false;
            user.RefreshToken = "";

            var userDto = await _userBusiness.UpdateUserAsync(user);
            if(userDto == null)
            {
                return false;
            }

            return true;
        }

        public async Task<AuthenticationToken?> RenewTokenAsync(string refreshToken)
        {
            var userDto = await _userBusiness.GetSingleUserWithPermissionAndRoleAsync("","",refreshToken);
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
                new Claim(nameof(user.FullName),user.FullName),
                new Claim(nameof(user.Email),user.Email),
                new Claim(nameof(user.Phone),user.Phone),
                new Claim(nameof(user.PhoneCode),user.PhoneCode),
                new Claim("userId",user.GUID),
                new Claim("permissions",JsonSerializer.Serialize(permissionItem)),
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
    }
}
