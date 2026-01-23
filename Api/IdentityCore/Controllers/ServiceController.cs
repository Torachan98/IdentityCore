using IdentityCore.Attributes;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Requests;

namespace IdentityCore.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService) 
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [Authorize]
        [Permission(Permission.FETCH, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<PaginationItems<ServiceDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServices([FromQuery] ServiceFetchRequest request)
        {
            return Ok(await _serviceService.GetAllAsync(request));
        }

        [HttpPost]
        [Authorize]
        [Permission(Permission.CREATE, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ServiceDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateService([FromBody] CreateOrUpdateServiceRequest request)
        {
            return Ok(await _serviceService.CreateAsync(request));
        }

        [HttpPut]
        [Authorize]
        [Permission(Permission.EDIT, Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<ServiceDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateService([FromBody] CreateOrUpdateServiceRequest request)
        {
            return Ok(await _serviceService.UpdateAsync(request));
        }

        [HttpDelete]
        [Authorize]
        [Permission(Permission.DELETE, Role.Administrator)]
        public async Task<IActionResult> DeleteService([FromQuery] Guid guid)
        {
            return Ok(await _serviceService.DeleteAsync(guid));
        }
    }
}
