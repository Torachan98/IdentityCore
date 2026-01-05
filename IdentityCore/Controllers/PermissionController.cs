using IdentityCore.EFs.DTOs;
using IdentityCore.Attributes;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService) 
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [Authorize]
        [Permission(Permission.FETCH, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<PaginationItems<PermissionDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissions([FromQuery] PermissionFetchRequest request)
        {
            return Ok(await _permissionService.GetAllAsync(request));
        }

        [HttpPost]
        [Authorize]
        [Permission(Permission.CREATE, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<PermissionDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePermission([FromBody] CreateOrUpdatePermissionRequest request)
        {
            return Ok(await _permissionService.CreateAsync(request));
        }

        [HttpPut]
        [Authorize]
        [Permission(Permission.EDIT, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<PermissionDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePermission([FromBody] CreateOrUpdatePermissionRequest request)
        {
            return Ok(await _permissionService.UpdateAsync(request));
        }

        [HttpDelete]
        [Authorize]
        [Permission(Permission.DELETE, Role.Administrator)]
        public async Task<IActionResult> DeletePermission([FromQuery] string guid)
        {
            return Ok(await _permissionService.DeleteAsync(guid));
        }
    }
}
