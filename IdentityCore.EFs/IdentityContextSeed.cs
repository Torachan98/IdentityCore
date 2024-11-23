using IdentityCore.EFs.DTOs;
using IdentityCore.EFs.Entities;
using IdentityCore.EFs.Enums;
using IdentityCore.EFs.Helpers;
namespace IdentityCore.EFs
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(IdentityContext context)
        {
            var taskUser = InitialUser(context);
            var taskPermission = InitialPermission(context);
            var taskRole = InitialRole(context);

            await Task.WhenAll(taskUser, taskPermission, taskRole);
            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        private static async Task InitialUser(IdentityContext context)
        {
            var userItems = new List<UserEntity>() {
                new UserEntity()
                {
                    FirstName = "",
                    LastName = "",
                    MiddleName = "",
                    UserName = "Administrator",
                    Email = "baovkg@gmail.com",
                    Password = EnscryptHelper.ConvertSHA256("Aa@123456"),
                    Phone = "0908768106",
                    PhoneCode = "+84",
                    Region = "VN",
                }
            };

            var userData = userItems.Where(s => !context.Users.Any(r => s.UserName == r.UserName)).ToList();
            if (userData.Count > 0)
            {
                await context.Users.AddRangeAsync(userData);
            }
        }

        private static async Task InitialPermission(IdentityContext context)
        {
            var permissions = EnumHelper.ConvertPermissionToList<Permission, PermissionEnum>();
            if (permissions != null)
            {
                var permissionData = permissions.Where(s => !context.Permissions.Any(r => s.Permission.ToString() == r.Name))
                                                .Select(s => new PermissionEntity()
                                                {
                                                    PermissionType = s.PermissionType,
                                                    Description = s.Description,
                                                    Name = s.Permission.ToString(),

                                                }).ToList();
                if (permissionData.Count > 0)
                {
                    await context.Permissions.AddRangeAsync(permissionData);
                }
            }
        }

        private static async Task InitialRole(IdentityContext context)
        {
            var roleItems = EnumHelper.ConvertRoleToList<Role, RoleEnum>();
            if (roleItems != null)
            {
                var roleData = roleItems.Where(s => !context.Roles.Any(r => s.Role.ToString() == r.RoleName))
                                        .Select(s=> new RoleEntity()
                                        {
                                            RoleName = s.Role.ToString(),
                                            Description = s.Description,
                                        }).ToList();

                if (roleData.Count > 0)
                {
                    await context.Roles.AddRangeAsync(roleData);
                }
            }
        }
    }
}
