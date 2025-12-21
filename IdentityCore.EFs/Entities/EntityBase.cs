namespace IdentityCore.EFs.Entities
{
    public class EntityBase : IBaseEntity
    {
        private string? _guid;

        public string GUID {
            get 
            {
                if (_guid == null) 
                {
                    return _guid = Guid.NewGuid().ToString().ToUpper();
                }
                else
                {
                    return _guid.ToUpper();
                }
            }
            set
            {
                if(value == null)
                {
                    _guid = Guid.NewGuid().ToString().ToUpper();
                }
                else
                {
                    _guid = value.ToUpper();
                }
            }
        }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
