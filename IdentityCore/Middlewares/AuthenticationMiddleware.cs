using IdentityCore.Business.Interfaces;
using IdentityCore.EFs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IdentityCore.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<GlobalHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GlobalConfiguration.Jwt.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidAudience = GlobalConfiguration.Jwt.Audience,
                    ValidIssuer = GlobalConfiguration.Jwt.Issuer
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var guid = jwtToken.Claims.First(x => x.Type == "userId").Value;
                var timeStamp = jwtToken.Claims.First(x => x.Type == "exp").Value;

                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                DateTime dateExpired = dateTime.AddSeconds(double.Parse(timeStamp)).ToUniversalTime();

                if (dateExpired > DateTime.UtcNow)
                {
                    var services = context.RequestServices;
                    var _userBusiness = (IUserBusiness)services.GetService(typeof(IUserBusiness));

                    var user = await _userBusiness.GetSingleUserWithPermissionAndRoleAsync("",guid);
                    context.Items["User"] = user;
                }
                else
                {
                    context.Items["User"] = null;
                }
            }

            await _next(context);
        }
    }
}
