using IdentityCore.EFs.Enums;

namespace IdentityCore.EFs.DTOs
{
    public class RoleDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RoleEnum
    {
        public Role Role { get; set; }
        public string Description { get; set; }
    }

    public class CreateOrUpdateRoleRequest
    {

    }

    public class RoleFetchRequest
    {

    }
}
