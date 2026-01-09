using IdentityCore.EFs.DTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Requests
{
    public class UserInfoRequest
    {
        public string Jwt { get; set; } = string.Empty;
    }

    public class UserInfoResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<RoleEnum> Roles { get; set; }
    }

    public class FileRequest
    {
        public int FileType { get; set; }
        public IFormFile File { get; set; }
    }
}
