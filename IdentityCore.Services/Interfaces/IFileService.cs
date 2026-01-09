using IdentityCore.EFs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(FileType fileType, IFormFile file);
        Task<MemoryStream> ViewImageAsync(string fileId);
        
    }
}
