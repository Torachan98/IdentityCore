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
using IdentityCore.EFs.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IdentityCore.Business
{
    public class AuthenticationBusiness : BaseBusiness, IAuthenticationBusiness
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IUserRepository _userRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserServiceRepository _userServiceRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        
        public AuthenticationBusiness(IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IDistributedCache distributedCache,
            IHttpContextAccessor httpContextAccessor,
            IUserBusiness userBusiness,
            IEmailBusiness emailBusiness, 
            IUserRepository userRepository,
            IServiceRepository serviceRepository,
            IUserServiceRepository userServiceRepository,
            IUserPermissionRepository userPermissionRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
            _userRepository = userRepository;
            _serviceRepository = serviceRepository;
            _userServiceRepository = userServiceRepository;
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task<ObjectResponse<AuthenticationToken>> SignInAsync(SignInRequest signInRequest)
        {
            var user = await _userBusiness.GetSingleUserWithPermissionAndRoleAsync(signInRequest.UserName!,signInRequest.AppKeys);
            var passwordEnscrypt = EnscryptHelper.ConvertSHA256(signInRequest.Password!);

            if(user == null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "User or password is not corrected");
            }

            if (!Equals(user.Password, passwordEnscrypt))
            {
                if(user.AttemptLogin == GlobalConst.AccountLocked.AttemptNumber)
                {
                    user.Locked = DateTime.UtcNow.AddHours(GlobalConst.AccountLocked.LockedHour);
                    await _userBusiness.UpdateUserAsync(user);
                    throw new FriendlyException(StatusCodes.Status423Locked, "This user has been locked");
                }

                user.AttemptLogin = user.AttemptLogin + 1;

                await _userBusiness.UpdateUserAsync(user);

                throw new FriendlyException(StatusCodes.Status400BadRequest, "User or password is not corrected");
            }

            if(user.Locked > DateTime.UtcNow)
            {
                throw new FriendlyException(StatusCodes.Status423Locked, "This user has been locked");
            }

            if (user.IsActive.HasValue && !user.IsActive.Value)
            {
                user.OTPCode = _emailBusiness.GenerateOTP(GlobalConst.OTP.SizeCode);
                user.OTPLifeTime = DateTime.UtcNow.AddMinutes(GlobalConst.OTP.LifeTimeMinute);

                await _emailBusiness.SendMailAsync(user, TemplateEmailType.OTP);
                await _userBusiness.UpdateUserAsync(user);

                return new ObjectResponse<AuthenticationToken>()
                {
                    Item = new AuthenticationToken()
                    {
                        Step = user.Step ?? (int)Step.WaitingConfirmed,
                        AccessToken = null,
                        RefreshToken = null
                    },
                    Message = "User is not active"
                };
            }

            if (user.IsLogin != null && user.IsLogin.Value)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "User already login");
            }

            var result = new AuthenticationToken()
            {
                RefreshToken = GenerateRefreshToken(),
                AccessToken = GenerateToken(user),
                Step = user.Step ?? (int)Step.Verified
            };

            user.IsLogin = true;
            user.RefreshToken = result.RefreshToken;

            await _userBusiness.UpdateUserAsync(user);
            await _distributedCache.SetStringAsync($"{KeyCache.User}-{user.GUID}", JsonConvert.SerializeObject(user));

            return new ObjectResponse<AuthenticationToken>()
            {
                Item = result
            };
        }

        public async Task<bool> SignOutAsync(UserDTO user, string accessToken)
        {
            user.IsLogin = false;
            user.RefreshToken = null;

            var userDto = await _userBusiness.UpdateUserAsync(user);
            if(userDto == null)
            {
                return false;
            }

            var tokenBlacklist = await _distributedCache.GetStringAsync(KeyCache.BlackList) ?? "";
            var blacklist = !string.IsNullOrEmpty(tokenBlacklist) ? JsonConvert.DeserializeObject<List<TokenBlacklist>>(tokenBlacklist) : new List<TokenBlacklist>();
            blacklist!.Add(new TokenBlacklist() { Token = accessToken, DateExpired = FetchSessionToken(accessToken) });
            await _distributedCache.SetStringAsync(KeyCache.BlackList, JsonConvert.SerializeObject(blacklist));
            await _distributedCache.RemoveAsync($"{KeyCache.User}-{user.GUID}");
            return true;
        }

        public async Task<AuthenticationToken> RenewTokenAsync(string refreshToken)
        {
            var appKeys = _httpContextAccessor.HttpContext!.Items["AppKeys"] as List<string>;
            var userDto = await _userBusiness.GetSingleUserWithPermissionAndRoleAsync("", appKeys ?? new List<string>(), null,refreshToken);

            if (userDto == null)
            {
                throw new FriendlyException(StatusCodes.Status401Unauthorized, "Refresh token invalid");
            }

            if (userDto.Locked.HasValue && userDto.Locked >= DateTime.UtcNow)
            {
                throw new FriendlyException(StatusCodes.Status401Unauthorized, "User has been locked");
            }

            await _distributedCache.SetStringAsync($"{KeyCache.User}-{userDto.GUID}", JsonConvert.SerializeObject(userDto));

            return new AuthenticationToken()
            {
                RefreshToken = userDto.RefreshToken,
                AccessToken = GenerateToken(userDto)
            };
        }

        public async Task<bool> ConfirmOTPAsync(string otpCode)
        {
            if (string.IsNullOrEmpty(otpCode))
            {
                return false;
            }

            var userEntity = await _userRepository.Get(s => s.OTPCode == otpCode && !s.IsActive && !s.IsDeleted).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                return false;
            }

            if (userEntity.OTPLifeTime > DateTime.UtcNow)
            {
                await _userBusiness.UpdateUserAsync(new UserDTO()
                {
                    GUID = userEntity.GUID,
                    Step = (int)Step.Verified,
                    IsActive = true
                });

                return true;
            }


            userEntity.OTPCode = _emailBusiness.GenerateOTP(GlobalConst.OTP.SizeCode);
            userEntity.OTPLifeTime = DateTime.UtcNow.AddMinutes(GlobalConst.OTP.LifeTimeMinute);

            var userDto = _mapper.Map<UserDTO>(userEntity);
            await _emailBusiness.SendMailAsync(userDto, TemplateEmailType.OTP);
            await _userBusiness.UpdateUserAsync(userDto);

            return false;
        }

        public async Task ResetEmailAsync(string email)
        {
            var userEntity = await _userRepository.Get().FirstOrDefaultAsync(s => s.Email == email && s.IsActive && !s.IsDeleted);
            if (userEntity == null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "User not valid");
            }

            userEntity.OTPCode = _emailBusiness.GenerateOTP(5);
            userEntity.OTPLifeTime = DateTime.UtcNow.AddMinutes(15);
            _userRepository.Add(userEntity);

            var userDto = _mapper.Map<UserDTO>(userEntity);
            await _emailBusiness.SendMailAsync(userDto, TemplateEmailType.OTP);
            await _unitOfWork.CommitAsync();
        }

        public async Task ResetEmailConfirmAsync(string email, string otpCode)
        {
            var userEntity = await _userRepository.Get().FirstOrDefaultAsync(s => s.Email == email &&
                                                                            s.OTPCode == otpCode &&
                                                                            s.OTPLifeTime.HasValue && s.OTPLifeTime >= DateTime.UtcNow &&
                                                                            s.IsActive && !s.IsDeleted);
            if (userEntity == null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "User not valid");
            }

            userEntity.Email = email;
            userEntity.OTPLifeTime = DateTime.UtcNow;
            _userRepository.Update(userEntity, s => s.Email);
            await _unitOfWork.CommitAsync();
        }

        public async Task ResetPasswordAsync(string email)
        {
            var userEntity = await _userRepository.Get().FirstOrDefaultAsync(s => s.Email == email && s.IsActive && !s.IsDeleted);
            if(userEntity == null)
            {
                throw new FriendlyException(StatusCodes.Status400BadRequest, "User not valid");
            }

            userEntity.OTPCode = _emailBusiness.GenerateOTP(5);
            userEntity.OTPLifeTime = DateTime.UtcNow.AddMinutes(15);
            _userRepository.Add(userEntity);

            var userDto = _mapper.Map<UserDTO>(userEntity);
            await _emailBusiness.SendMailAsync(userDto, TemplateEmailType.OTP);
            await _unitOfWork.CommitAsync();
        }

        public Task ResetPasswordConfirmAsync(string password, string otpCode)
        {
            throw new NotImplementedException();
        }

        private DateTime FetchSessionToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GlobalConst.Jwt.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidAudience = GlobalConst.Jwt.Audience,
                ValidIssuer = GlobalConst.Jwt.Issuer
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var timeStamp = jwtToken.Claims.First(x => x.Type == "exp").Value;
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateExpired = dateTime.AddSeconds(double.Parse(timeStamp)).ToUniversalTime();
            return dateExpired;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateToken(UserDTO user)
        {
            var claims = new[] {
                new Claim("firstName",user.FirstName!),
                new Claim("lastName",user.LastName!),
                new Claim("email",user.Email!),
                new Claim("phone",$"{user.PhoneCode} {user.Phone}"),
                new Claim("userId",user.GUID.ToString()),
                new Claim("permissions",JsonConvert.SerializeObject(user.Permissions)),
                new Claim("roles",JsonConvert.SerializeObject(user.Roles)),
                new Claim("services",JsonConvert.SerializeObject(user.Services)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalConst.Jwt.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(GlobalConst.Jwt.Issuer,
                                              GlobalConst.Jwt.Audience,
                                              claims,
                                              expires: DateTime.UtcNow.AddMinutes(GlobalConst.Jwt.LifeTime),
                                              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
