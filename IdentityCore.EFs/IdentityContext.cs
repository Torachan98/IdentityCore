using IdentityCore.EFs.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityCore.EFs
{
    #pragma warning disable CS1591
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) 
        {
 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasMany(p => p.UserRoles).WithOne(r => r.Users).HasForeignKey(c => c.UserId);
                entity.HasMany(p => p.UserServices).WithOne(r => r.Users).HasForeignKey(c => c.UserId);
                entity.HasMany(p => p.UserPermissions).WithOne(r => r.Users).HasForeignKey(c => c.UserId);
            });


            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.HasMany(p => p.RolePermissions).WithOne(r => r.Roles).HasForeignKey(c => c.RoleId);
                entity.HasMany(p => p.UserRoles).WithOne(r => r.Roles).HasForeignKey(c => c.RoleId);
            });

            modelBuilder.Entity<PermissionEntity>(entity =>
            {
                entity.HasMany(p => p.RolePermissions).WithOne(r => r.Permissions).HasForeignKey(c => c.PermissionId);
                entity.HasMany(p => p.UserPermissions).WithOne(r => r.Permissions).HasForeignKey(c => c.PermissionId);
            });

            modelBuilder.Entity<ServiceEntity>().HasMany(p => p.UserServices).WithOne(r => r.Services).HasForeignKey(c => c.ServiceId);        
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }
        public DbSet<UserPermissionEntity> UserPermissions { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<UserServiceEntity> UserServices { get; set; }
    }
    #pragma warning restore CS1591
}
