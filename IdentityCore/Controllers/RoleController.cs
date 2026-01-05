using IdentityCore.Attributes;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize]
        [Permission(Permission.FETCH, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<PaginationItems<RoleDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles([FromQuery] RoleFetchRequest request)
        {
            return Ok(await _roleService.GetAllAsync(request));
        }

        [HttpPost]
        [Authorize]
        [Permission(Permission.CREATE, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<RoleDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRole([FromBody] CreateOrUpdateRoleRequest request)
        {
            return Ok(await _roleService.CreateAsync(request));
        }

        [HttpPut]
        [Authorize]
        [Permission(Permission.EDIT, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<PermissionDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePermission([FromBody] CreateOrUpdateRoleRequest request)
        {
            return Ok(await _roleService.UpdateAsync(request));
        }

        [HttpDelete]
        [Authorize]
        [Permission(Permission.DELETE, Role.Administrator)]
        public async Task<IActionResult> DeleteRole([FromQuery] string guid)
        {
            return Ok(await _roleService.DeleteAsync(guid));
        }
    }
}
