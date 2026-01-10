using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityCore.Attributes
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(Permission permission, Role role) : base(typeof(PermissionActionFilter))
        {
            Arguments = new object[] { permission, role };
        }

        public class PermissionActionFilter : IAuthorizationFilter
        {
            private readonly Permission _permission;
            private readonly Role _role;

            public PermissionActionFilter(Permission permission, Role role)
            {
                _permission = permission;
                _role = role;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.Items["User"] as UserDTO;                
                if (user == null)
                {
                    context.Result = new JsonResult(new { isSuccess = false, requestId = Guid.NewGuid().ToString(), code = StatusCodes.Status400BadRequest, message = "User do not have permission" });
                    return;
                }

                var isUserHaveRole = user.Roles.Where(s => s.Permissions.Any(p => p == _permission.ToString())).FirstOrDefault();
                var isUserHavePermission = user.Permissions.Where(u => u == _permission.ToString()).FirstOrDefault();


                if (isUserHaveRole == null && isUserHavePermission == null)
                {
                    context.Result = new JsonResult(new { isSuccess = false, requestId = Guid.NewGuid().ToString(), code = StatusCodes.Status400BadRequest, message = "User do not have permission" });
                    return;
                }
            }
        }
    }
}
