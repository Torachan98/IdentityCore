using IdentityCore.EFs.Requests;
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
        public async Task<IActionResult> GetPermissions()
        {
            //var result = await _userService.GetUsers(userFetchRequest);
            return Ok("OK");
        }
    }
}
