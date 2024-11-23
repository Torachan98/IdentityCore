using IdentityCore.EFs.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = context.HttpContext.Items["User"] as UserDTO;

            if (account == null || (bool)!account.IsLogin)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
