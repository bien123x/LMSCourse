using LMSCourse.Migrations;
using LMSCourse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineCourseConstants;

namespace LMSCourse.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<PasswordPolicy> PasswordPolicies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Cấu hình
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey("UserId");

                entity.HasIndex(u => u.UserName).IsUnique();

                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                      .IsRequired();

                entity.Property(u => u.CreationTime)
                        .HasDefaultValueSql("GETDATE()"); // SQL Server, DB mặc định UTC hiện tại

                entity.Property(u => u.ModificationTime)
                       .HasDefaultValueSql("GETDATE()");

                entity.Property(u => u.PasswordUpdateTime)
                       .HasDefaultValueSql("GETDATE()");

                entity.Property(u => u.FailedAccessCount)
                       .HasDefaultValue(0);
            });

           

            modelBuilder.Entity<PasswordPolicy>()
                .HasData(
                    new PasswordPolicy
                    {
                        PasswordPolicyId = 1,
                        MinLength = 6,
                        RequiredUniqueChars = 3,
                        RequireDigit = true,
                        RequireLowercase = true,
                        RequireUppercase = true,
                        RequireNonAlphanumeric = true,
                    }
                );
        }
    }
}
