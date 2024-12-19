using IdentityCore.EFs.DTOs;
using IdentityCore.Attributes;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetPermissions([FromQuery] PermissionFetchRequest request)
        {
            return Ok(await _permissionService.GetAllAsync(request));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePermission([FromBody] CreateOrUpdatePermissionRequest request)
        {
            return Ok(await _permissionService.CreateAsync(request));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePermission([FromBody] CreateOrUpdatePermissionRequest request)
        {
            return Ok(await _permissionService.UpdateAsync(request));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePermission([FromQuery] string guid)
        {
            return Ok(await _permissionService.DeleteAsync(guid));
        }
    }
}
