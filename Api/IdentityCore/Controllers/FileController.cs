using IdentityCore.Attributes;
using IdentityCore.EFs;
using IdentityCore.EFs.Requests;
using IdentityCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromForm] FileRequest fileRequest)
        {
            if(fileRequest.File == null || fileRequest.File.Length == 0)
            {
                throw new FriendlyException(StatusCodes.Status204NoContent, "No file uploaded or file is empty.");
            }

            return Ok(await _fileService.UploadFileAsync((FileType)fileRequest.FileType, fileRequest.File));
        }

        //[HttpGet("{fileId}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> ViewDocument([FromRoute] string fileId, [FromQuery] int type)
        //{
        //    var result = await _fileService.ViewImageAsync(fileId);
        //    return new FileStreamResult(result, "image/jpeg");
        //}
    }
}
