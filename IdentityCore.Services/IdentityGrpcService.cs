using AutoMapper;
using Grpc.Core;
using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Requests;
using IdentityCore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace IdentityCore.Services
{
    public class IdentityGrpcService: Identity.IdentityBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _users;
        private readonly IRoleRepository _role;

        public IdentityGrpcService(IMapper mapper, IUserRepository users, IRoleRepository role)
        {
            _mapper = mapper;
            _users = users;
            _role = role;
        }

        public override async Task<UserInfoResponse> GetUserInfo(UserInfoRequest request,ServerCallContext context)
        {
            ClaimsPrincipal principal;

            try
            {
                principal = ValidateJwt(request.AccessToken);
            }
            catch
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid JWT"));
            }

            var userId = principal.Claims.ToList().Find(s => s.Type == "userId").Value;

            var user = await _users.Get(s => s.GUID == userId)
                                    .Include(r => r.UserRoles)
                                    .Include(s => s.UserServices)
                                    .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            var roleIds = user.UserRoles.Select(r => r.RoleId).ToList();
            var roleName = await _role.Get(s => roleIds.Contains(s.RoleId)).Select(s => s.Name).ToListAsync();
            var userDto = _mapper.Map<UserDTO>(user);

            if(userDto is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }

            return new UserInfoResponse
            {
                UserId = userDto.GUID,
                Email = userDto.Email,
                Roles = JsonSerializer.Serialize(roleName),
                Status = userDto.IsActive ?? false
            };
        }

        private ClaimsPrincipal ValidateJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = GlobalConst.Jwt.Audience,
                ValidIssuer = GlobalConst.Jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(GlobalConst.Jwt.Key)
                )
            }, out _);
        }
    }
}
