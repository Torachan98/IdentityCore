using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Requests;

namespace IdentityCore.EFs.DTOs
{
    public class ServiceDTO
    {
        public Guid? GUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SignatureKey { get; set; }

        public List<UserServiceDTO> UserServices { get; set; } = new List<UserServiceDTO>();
    }

    public class UserServiceDTO
    {
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public DateTimeOffset? DateActive { get; set; }
        public DateTimeOffset? DateExpired { get; set; }
    }

    public class CreateOrUpdateServiceRequest
    {
        public Guid? GUID { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public string? SignatureKey { get; set; }
    }

    public class ServiceFetchRequest: FetchParams
    {

    }


    public class AssigningService
    {
        public Guid? GUID { get; set; } = Guid.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Key { get; set; } = string.Empty;
        public DateTimeOffset? DateActive { get; set; }
        public DateTimeOffset? DateExpired { get; set; }
    }
}
