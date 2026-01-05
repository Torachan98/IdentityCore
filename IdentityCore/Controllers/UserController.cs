using Hangfire.MemoryStorage.Dto;
using IdentityCore.Attributes;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [Permission(Permission.FETCH,Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<PaginationItems<UserDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromQuery] UserFetchRequest request)
        {
            return Ok(await _userService.GetAllAsync(request));
        }

        [HttpGet("{id}", Name= "GetUserById")]
        [Authorize]
        [Permission(Permission.FETCH, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        [Permission(Permission.CREATE, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser([FromBody] CreateOrUpdateUserRequest request)
        {
            return Ok(await _userService.CreateAsync(request));
        }

        [HttpPut]
        [Authorize]
        [Permission(Permission.EDIT, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] CreateOrUpdateUserRequest request)
        {
            return Ok(await _userService.UpdateAsync(request));
        }

        [HttpDelete]
        [Authorize]
        [Permission(Permission.DELETE, Role.Administrator)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser([FromQuery] string guid)
        {
            return Ok(await _userService.DeleteAsync(guid));
        }
    }
}
