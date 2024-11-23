namespace IdentityCore.EFs.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class LockedAttribute: Attribute
    {
        public bool IsLock { get; }

        public LockedAttribute(bool isLock = false)
        {
            IsLock = isLock;
        }
    }
}
