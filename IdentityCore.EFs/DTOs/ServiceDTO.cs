using IdentityCore.EFs.Requests;

namespace IdentityCore.EFs.DTOs
{
    public class ServiceDTO
    {
        public string? GUID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SignatureKey { get; set; }
    }

    public class CreateOrUpdateServiceRequest
    {
        public string? GUID { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public string? SignatureKey { get; set; }
    }

    public class ServiceFetchRequest: FetchParams
    {

    }
}
