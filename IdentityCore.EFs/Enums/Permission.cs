using IdentityCore.EFs.Attributes;
using System.ComponentModel;

namespace IdentityCore.EFs.Enums
{
    public enum Permission
    {
        [Permission(PermissionType.UIAccess)]
        [Description("This permission is used for sign in, sign out, fetch token")]
        AUTHENTICATION,

        [Permission(PermissionType.Services)]
        [Description("Allow user can get a specific item in a module")]
        FETCH,

        [Permission(PermissionType.Services)]
        [Description("Allow user can create a specific item in a module")]
        CREATE,

        [Permission(PermissionType.Services)]
        [Description("Allow user can edit a specific item in a module")]
        EDIT,

        [Permission(PermissionType.Services)]
        [Description("Allow user can delete a specific item in a module")]
        DELETE,

        [Permission(PermissionType.Services)]
        [Description("Allow user can lock a specific item in a module")]
        LOCK,

        [Permission(PermissionType.UIAccess)]
        [Description("Allow user can get a specific item in a module")]
        FETCH_UI_ACCESS,

        [Permission(PermissionType.UIAccess)]
        [Description("Allow user can create a specific item in a module")]
        CREATE_UI_ACCESS,

        [Permission(PermissionType.UIAccess)]
        [Description("Allow user can edit a specific item in a module")]
        EDIT_UI_ACCESS,

        [Permission(PermissionType.UIAccess)]
        [Description("Allow user can delete a specific item in a module")]
        DELETE_UI_ACCESS,
    }
}
