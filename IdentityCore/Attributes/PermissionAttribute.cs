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
                    context.Result = new JsonResult(new { isSucess = false, requestId = Guid.NewGuid().ToString(), code = StatusCodes.Status400BadRequest, message = "User do not have permission" });
                    return;
                }

                var isRoleHavePermission = user.GroupRoles.Select(s => s.Role).Any(s => s >= _role);
                if (!isRoleHavePermission)
                {
                    context.Result = new JsonResult(new { isSucess = false, requestId = Guid.NewGuid().ToString(), code = StatusCodes.Status400BadRequest, message = "User do not have permission" });
                    return;
                }

                var isPermissionRole = user.GroupRoles.Where(s => s.Permissions!.Any(x => x.Permission == _permission)).Any();
                var isPermissionExisted = user.GroupPermissions.Any(s => s.Permission == _permission);
                if (!isPermissionExisted && !isPermissionRole)
                {
                    context.Result = new JsonResult(new { isSucess = false, requestId = Guid.NewGuid().ToString(), code = StatusCodes.Status400BadRequest, message = "User do not have permission" });
                    return;
                }
            }
        }
    }
}
