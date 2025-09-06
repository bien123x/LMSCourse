using LMSCourse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

                entity.HasData(new User
                {
                    UserId = 1,
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123"),
                    PhoneNumber = "01234456",
                    IsActive = true
                });
            });
        }
    }
}
