using IdentityCore.Attributes;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IdentityCore.EFs.DTOs;

namespace IdentityCore.Controllers
{
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService) 
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetServices([FromQuery] ServiceFetchRequest request)
        {
            return Ok(await _serviceService.GetAllAsync(request));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateService([FromBody] CreateOrUpdateServiceRequest request)
        {
            return Ok(await _serviceService.CreateAsync(request));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateService([FromBody] CreateOrUpdateServiceRequest request)
        {
            return Ok(await _serviceService.UpdateAsync(request));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteService([FromQuery] string guid)
        {
            return Ok(await _serviceService.DeleteAsync(guid));
        }
    }
}
