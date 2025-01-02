using IdentityCore.EFs;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace IdentityCore.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;
        public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<GlobalHandlerMiddleware> logger, IDistributedCache distributedCache)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
            _distributedCache = distributedCache;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var blackListString = await _distributedCache.GetStringAsync(KeyCache.BlackList);
                var blackList = !string.IsNullOrEmpty(blackListString) ? JsonSerializer.Deserialize<List<TokenBlacklist>>(blackListString)! : new List<TokenBlacklist>();
                if (!blackList.Any(s => s.Token.Equals(token))) 
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
                    var guid = jwtToken.Claims.First(x => x.Type == "userId").Value;
                    var timeStamp = jwtToken.Claims.First(x => x.Type == "exp").Value;

                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    DateTime dateExpired = dateTime.AddSeconds(double.Parse(timeStamp)).ToUniversalTime();

                    if (dateExpired > DateTime.UtcNow)
                    {
                        var user = await _distributedCache.GetStringAsync($"{KeyCache.User}-{guid}");
                        if (user != null)
                        {
                            context.Items["User"] = JsonSerializer.Deserialize<UserDTO>(user);
                        }
                    }
                    else
                    {
                        context.Items["User"] = null;
                    }
                }
            }

            await _next(context);
        }
    }
}
