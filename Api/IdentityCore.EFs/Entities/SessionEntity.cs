using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityCore.EFs.Entities
{
    public class SessionEntity : EntityBase
    {
        [Key]
        public int SessionId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public string OSSystem { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public string DeviceID { get; set; } = string.Empty;
        public string MACAddress { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool IsLock { get; set; }

        public virtual UserEntity Users { get; set; }
    }
}
