using System.ComponentModel;

namespace IdentityCore.EFs.Enums
{
    public enum Role
    {
        [Description("Administrator")]
        Administrator = 999,

        [Description("Guest")]
        Guest = 0,


        [Description("User")]
        User = 1
    }
}
