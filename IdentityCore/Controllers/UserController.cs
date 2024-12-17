using IdentityCore.Attributes;
using IdentityCore.EFs.DTOs;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCore.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromQuery] UserFetchRequest request)
        {
            return Ok(await _userService.GetAllAsync(request));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateOrUpdateUserRequest request)
        {
            return Ok(await _userService.CreateAsync(request));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] CreateOrUpdateUserRequest request)
        {
            return Ok(await _userService.UpdateAsync(request));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromQuery] string guid)
        {
            return Ok(await _userService.DeleteAsync(guid));
        }
    }
}
