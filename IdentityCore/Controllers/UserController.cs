using IdentityCore.Attributes;
using IdentityCore.EFs.Requests;
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
        public async Task<IActionResult> GetUsers([FromQuery] UserFetchRequest userFetchRequest)
        {
            var result = await _userService.GetUsersAsync(userFetchRequest);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            var result = await _userService.CreateUserAsync(createUserRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest)
        {
            var result = await _userService.UpdateUserAsync(userRequest);
            return Ok(result);
        }
    }
}
