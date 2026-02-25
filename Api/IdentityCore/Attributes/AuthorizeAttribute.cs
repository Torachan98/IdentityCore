using IdentityCore.EFs.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using IdentityCore.EFs.Entities;

namespace IdentityCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = context.HttpContext.Items["User"] as UserDTO;
            //var session = context.HttpContext.Items["User"] as SessionEntity;

            if (account == null)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
