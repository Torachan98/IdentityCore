using System.ComponentModel;

namespace IdentityCore.EFs.Enums
{
    public enum Permission
    {
        [Description("This permission is used for sign in, sign out, fetch token")]
        AUTHENTICATION,

        [Description("Allow user can get a specific item in a module")]
        FETCH,

        [Description("Allow user can create a specific item in a module")]
        CREATE,

        [Description("Allow user can edit a specific item in a module")]
        EDIT,

        [Description("Allow user can delete a specific item in a module")]
        DELETE,

        [Description("Allow user can lock a specific item in a module")]
        LOCK,
    }
}
