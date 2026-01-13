namespace IdentityCore.EFs.Entities
{
    public class EntityBase : IBaseEntity
    {
        public Guid GUID { get; set; } = Guid.NewGuid();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
