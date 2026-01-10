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
                    FirstName = "Japper",
                    LastName = "Woody",
                    MiddleName = ".T",
                    UserName = "Administrator",
                    Email = "baovkg@gmail.com",
                    Password = EnscryptHelper.ConvertSHA256("Aa@123456"),
                    Phone = "0908768106",
                    PhoneCode = "+84",
                    Region = "VN",
                    Step = (int)Step.WaitingConfirmed,
                }
            };

            var userData = userItems.Where(s => !context.Users.Any(r => s.UserName == r.UserName)).ToList();
            if (userData.Count == 0)
            {
                return;
            }

            await context.Users.AddRangeAsync(userData);
        }

        private static async Task InitialPermission(IdentityContext context)
        {
            var permissions = EnumHelper.ConvertPermissionToList<Permission, PermissionEnum>();
            if (permissions != null)
            {
                var permissionData = permissions.Where(s => !context.Permissions.Any(r => s.Value == r.Value))
                                                .Select(s => new PermissionEntity()
                                                {
                                                    Value = s.Value,
                                                    Description = s.Description,
                                                    Name = s.Name,

                                                }).ToList();
                if (permissionData.Count == 0)
                {
                    return;
                }

                await context.Permissions.AddRangeAsync(permissionData);
            }
        }

        private static async Task InitialRole(IdentityContext context)
        {
            var roleItems = EnumHelper.ConvertRoleToList<Role, RoleEnum>();
            if (roleItems != null)
            {
                var roleData = roleItems.Where(s => !context.Roles.Any(r => s.Value == r.Value))
                                        .Select(s => new RoleEntity()
                                        {
                                            Name = s.Name,
                                            Description = s.Description,
                                            Value = s.Value
                                        }).ToList();

                if (roleData.Count == 0)
                {
                    return;
                }

                await context.Roles.AddRangeAsync(roleData);
            }
        }
    }
}
