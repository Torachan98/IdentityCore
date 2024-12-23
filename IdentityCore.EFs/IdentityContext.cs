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
            modelBuilder.Entity<UserEntity>()
            .HasMany(p => p.UserRolePermissions)
            .WithOne(r => r.Users)
            .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<RoleEntity>()
            .HasMany(p => p.UserRolePermissions)
            .WithOne(r => r.Roles)
            .HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<PermissionEntity>()
            .HasOne(p => p.UserRolePermissions)
            .WithOne(r => r.Permissions)
            .HasForeignKey<PermissionEntity>(c => c.PermissionId);

            modelBuilder.Entity<UserRolePermissionEntity>(entity =>
            {
                entity.HasOne(p => p.Users).WithMany(r => r.UserRolePermissions).HasForeignKey(c => c.UserId);
                entity.HasOne(p => p.Permissions).WithOne(r => r.UserRolePermissions).HasForeignKey<UserRolePermissionEntity>(c => c.PermissionId);
                entity.HasOne(p => p.Roles).WithMany(r => r.UserRolePermissions).HasForeignKey(c => c.RoleId);
            });

            modelBuilder.Entity<UserServiceEntity>(entity =>
            {
                entity.HasOne(p => p.Users).WithMany(r => r.UserServices).HasForeignKey(c => c.UserId);
                entity.HasOne(p => p.Services).WithMany(r => r.UserServices).HasForeignKey(c => c.ServiceId);
            });            
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<UserRolePermissionEntity> UserRolePermissions { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<UserServiceEntity> UserServices { get; set; }
    }
    #pragma warning restore CS1591
}
