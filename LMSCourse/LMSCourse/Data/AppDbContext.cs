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

            modelBuilder.Entity<Role>()
                .HasData(
                    new Role { RoleId = 1, RoleName = "Admin" },
                    new Role { RoleId = 2, RoleName = "Teacher" },
                    new Role { RoleId = 3, RoleName = "Student" }
                );

            modelBuilder.Entity<Permission>()
                .HasData(
                    // Users
                    new Permission { PermissionId = 1, PermissionName = PERMISSION.ViewUsers},
                    new Permission { PermissionId = 2, PermissionName = PERMISSION.CreateUsers },
                    new Permission { PermissionId = 3, PermissionName = PERMISSION.EditUsers },
                    new Permission { PermissionId = 4, PermissionName = PERMISSION.DeleteUsers },

                    // Roles
                    new Permission { PermissionId = 5, PermissionName = PERMISSION.ViewRoles},
                    new Permission { PermissionId = 6, PermissionName = PERMISSION.CreateRoles },
                    new Permission { PermissionId = 7, PermissionName = PERMISSION.EditRoles },
                    new Permission { PermissionId = 8, PermissionName = PERMISSION.DeleteRoles },

                    // Courses
                    new Permission { PermissionId = 9, PermissionName = PERMISSION.ViewCourses },
                    new Permission { PermissionId = 10, PermissionName = PERMISSION.CreateCourses },
                    new Permission { PermissionId = 11, PermissionName = PERMISSION.EditCourses },
                    new Permission { PermissionId = 12, PermissionName = PERMISSION.DeleteCourses},

                    // Lessons
                    new Permission { PermissionId = 13, PermissionName = PERMISSION.ViewLessons },
                    new Permission { PermissionId = 14, PermissionName = PERMISSION.CreateLessons },
                    new Permission { PermissionId = 15, PermissionName = PERMISSION.EditLessons },
                    new Permission { PermissionId = 16, PermissionName = PERMISSION.DeleteLessons },

                    // Enrollments
                    new Permission { PermissionId = 17, PermissionName = PERMISSION.ViewEnrollments },
                    new Permission { PermissionId = 18, PermissionName = PERMISSION.ManageEnrollments },

                    // Payments
                    new Permission { PermissionId = 19, PermissionName = PERMISSION.ViewPayments },
                    new Permission { PermissionId = 20, PermissionName = PERMISSION.ManagePayments},

                    // Logs
                    new Permission { PermissionId = 21, PermissionName = PERMISSION.ViewLogs },

                    // System Permissions
                    new Permission { PermissionId = 22, PermissionName = PERMISSION.ViewPermissions },
                    new Permission { PermissionId = 23, PermissionName = PERMISSION.ManagePermissions }
                );

            modelBuilder.Entity<RolePermission>()
                .HasData(
                    // ===== Admin: full quyền =====
                    new RolePermission { RolePermissionId = 1, RoleId = 1, PermissionId = 1 },
                    new RolePermission { RolePermissionId = 2, RoleId = 1, PermissionId = 2 },
                    new RolePermission { RolePermissionId = 3, RoleId = 1, PermissionId = 3 },
                    new RolePermission { RolePermissionId = 4, RoleId = 1, PermissionId = 4 },
                    new RolePermission { RolePermissionId = 5, RoleId = 1, PermissionId = 5 },
                    new RolePermission { RolePermissionId = 6, RoleId = 1, PermissionId = 6 },
                    new RolePermission { RolePermissionId = 7, RoleId = 1, PermissionId = 7 },
                    new RolePermission { RolePermissionId = 8, RoleId = 1, PermissionId = 8 },
                    new RolePermission { RolePermissionId = 9, RoleId = 1, PermissionId = 9 },
                    new RolePermission { RolePermissionId = 10, RoleId = 1, PermissionId = 10 },
                    new RolePermission { RolePermissionId = 11, RoleId = 1, PermissionId = 11 },
                    new RolePermission { RolePermissionId = 12, RoleId = 1, PermissionId = 12 },
                    new RolePermission { RolePermissionId = 13, RoleId = 1, PermissionId = 13 },
                    new RolePermission { RolePermissionId = 14, RoleId = 1, PermissionId = 14 },
                    new RolePermission { RolePermissionId = 15, RoleId = 1, PermissionId = 15 },
                    new RolePermission { RolePermissionId = 16, RoleId = 1, PermissionId = 16 },
                    new RolePermission { RolePermissionId = 17, RoleId = 1, PermissionId = 17 },
                    new RolePermission { RolePermissionId = 18, RoleId = 1, PermissionId = 18 },
                    new RolePermission { RolePermissionId = 19, RoleId = 1, PermissionId = 19 },
                    new RolePermission { RolePermissionId = 20, RoleId = 1, PermissionId = 20 },
                    new RolePermission { RolePermissionId = 21, RoleId = 1, PermissionId = 21 },
                    new RolePermission { RolePermissionId = 22, RoleId = 1, PermissionId = 22 },
                    new RolePermission { RolePermissionId = 23, RoleId = 1, PermissionId = 23 },

                    // ===== Teacher =====
                    new RolePermission { RolePermissionId = 100, RoleId = 2, PermissionId = 9 },  // ViewCourses
                    new RolePermission { RolePermissionId = 101, RoleId = 2, PermissionId = 10 }, // CreateCourses
                    new RolePermission { RolePermissionId = 102, RoleId = 2, PermissionId = 11 }, // EditCourses
                    new RolePermission { RolePermissionId = 103, RoleId = 2, PermissionId = 12 }, // DeleteCourses

                    new RolePermission { RolePermissionId = 104, RoleId = 2, PermissionId = 13 }, // ViewLessons
                    new RolePermission { RolePermissionId = 105, RoleId = 2, PermissionId = 14 }, // CreateLessons
                    new RolePermission { RolePermissionId = 106, RoleId = 2, PermissionId = 15 }, // EditLessons
                    new RolePermission { RolePermissionId = 107, RoleId = 2, PermissionId = 16 }, // DeleteLessons

                    new RolePermission { RolePermissionId = 108, RoleId = 2, PermissionId = 17 }, // ViewEnrollments
                    new RolePermission { RolePermissionId = 109, RoleId = 2, PermissionId = 18 }, // ManageEnrollments

                    // ===== Student =====
                    new RolePermission { RolePermissionId = 200, RoleId = 3, PermissionId = 9 },  // ViewCourses
                    new RolePermission { RolePermissionId = 201, RoleId = 3, PermissionId = 13 }, // ViewLessons
                    new RolePermission { RolePermissionId = 202, RoleId = 3, PermissionId = 17 }  // ViewEnrollments
                );
            modelBuilder.Entity<UserRole>()
                .HasData(
                    new UserRole
                    {
                        UserRoleId = 1,
                        UserId = 1, // Admin User
                        RoleId = 1 // Admin
                    }
                );
        }
    }
}
