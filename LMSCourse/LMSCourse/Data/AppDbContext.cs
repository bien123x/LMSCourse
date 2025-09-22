using LMSCourse.Models;
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

        public DbSet<IdentitySetting> IdentitySettings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        
        public DbSet<Level> Levels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<CourseTopic> CourseTopics { get; set; }
        public DbSet<FaqItem> FaqItems { get; set; }
        public DbSet<FaqGroup> FaqGroups { get; set; }
        public DbSet<Course> Courses { get; set; }


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

                entity.Property(u => u.FailedAccessCount)
                       .HasDefaultValue(0);
            });

        }
    }
}
