namespace IdentityCore.EFs.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {

        public PermissionType PermissionType { get; }

        public PermissionAttribute(PermissionType permissionType)
        {
            PermissionType = permissionType;
        }
    }
}
