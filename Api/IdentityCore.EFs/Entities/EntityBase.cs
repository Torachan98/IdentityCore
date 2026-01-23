using System.ComponentModel;

namespace IdentityCore.EFs.Entities
{
    public class EntityBase : IBaseEntity
    {
        public Guid GUID { get; set; } = Guid.NewGuid();
        public DateTimeOffset DateCreated { get; set; } = DateTime.UtcNow;
        public DateTimeOffset DateModified { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
