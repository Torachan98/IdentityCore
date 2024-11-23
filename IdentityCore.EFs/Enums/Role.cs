using IdentityCore.EFs.Attributes;
using System.ComponentModel;

namespace IdentityCore.EFs.Enums
{
    public enum Role
    {
        [Description("Administrator")]
        [Locked]
        Administrator = 999,

        [Description("Guest")]
        [Locked]
        Guest = 0,


        [Description("User")]
        [Locked]
        User = 1
    }
}
