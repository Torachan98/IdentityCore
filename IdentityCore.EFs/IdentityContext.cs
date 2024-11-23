using IdentityCore.EFs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
            .HasForeignKey(c => c.UserId)
            .HasConstraintName("UserId");

            modelBuilder.Entity<RoleEntity>()
            .HasMany(p => p.UserRolePermissions)
            .WithOne(r => r.Roles)
            .HasForeignKey(c => c.RoleId)
            .HasConstraintName("RoleId");

            modelBuilder.Entity<PermissionEntity>()
            .HasOne(p => p.UserRolePermissions)
            .WithOne(r => r.Permissions)
            .HasForeignKey<PermissionEntity>(c => c.PermissionId)
            .HasConstraintName("PermissionId");

            modelBuilder.Entity<UserRolePermissionEntity>()
            .HasOne(p => p.Users)
            .WithMany(r => r.UserRolePermissions)
            .HasForeignKey(c => c.UserId)
            .HasConstraintName("UserId");

            modelBuilder.Entity<UserRolePermissionEntity>()
            .HasOne(p => p.Permissions)
            .WithOne(r => r.UserRolePermissions)
            .HasForeignKey<UserRolePermissionEntity>(c => c.PermissionId)
            .HasConstraintName("PermissionId");

            modelBuilder.Entity<UserRolePermissionEntity>()
            .HasOne(p => p.Roles)
            .WithMany(r => r.UserRolePermissions)
            .HasForeignKey(c => c.RoleId)
            .HasConstraintName("RoleId");
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<UserRolePermissionEntity> UserRolePermissions { get; set; }
    }
    #pragma warning restore CS1591
}
