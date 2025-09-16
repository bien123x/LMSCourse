using LMSCourse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineCourseConstants;

namespace LMSCourse.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Migrate DB nếu chưa
            await context.Database.MigrateAsync();

            // Seed Roles
            if (!context.Roles.Any())
            {
                var adminRole = new Role { RoleName = "Admin" };
                var studentRole = new Role { RoleName = "Student" };
                var teacherRole = new Role { RoleName = "Teacher" };

                context.Roles.AddRange(adminRole, studentRole, teacherRole);
                await context.SaveChangesAsync();
            }

            // Seed Admin User
            if (!context.Users.Any(u => u.Email == "bienhanoi123@gmail.com"))
            {
                var adminRole = await context.Roles.FirstAsync(r => r.RoleName == "Admin");

                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "bienhanoi123@gmail.com",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123")
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();

                // Gán Role cho User
                context.UserRoles.Add(new UserRole
                {
                    UserId = adminUser.UserId,
                    RoleId = adminRole.RoleId,
                });
                await context.SaveChangesAsync();
            }

            // Seed Permissions
            if (!context.Permissions.Any())
            {
                context.Permissions.AddRange(
                    // User management
                    new Permission { PermissionName = PERMISSION.ViewUsers },
                    new Permission { PermissionName = PERMISSION.CreateUsers },
                    new Permission { PermissionName = PERMISSION.EditUsers },
                    new Permission { PermissionName = PERMISSION.DeleteUsers },

                    // Role management
                    new Permission { PermissionName = PERMISSION.ViewRoles },
                    new Permission { PermissionName = PERMISSION.CreateRoles },
                    new Permission { PermissionName = PERMISSION.EditRoles },
                    new Permission { PermissionName = PERMISSION.DeleteRoles },

                    // Course management
                    new Permission { PermissionName = PERMISSION.ViewCourses },
                    new Permission { PermissionName = PERMISSION.CreateCourses },
                    new Permission { PermissionName = PERMISSION.EditCourses },
                    new Permission { PermissionName = PERMISSION.DeleteCourses },

                    // Lesson management
                    new Permission { PermissionName = PERMISSION.ViewLessons },
                    new Permission { PermissionName = PERMISSION.CreateLessons },
                    new Permission { PermissionName = PERMISSION.EditLessons },
                    new Permission { PermissionName = PERMISSION.DeleteLessons },

                    // Enrollment management
                    new Permission { PermissionName = PERMISSION.ViewEnrollments },
                    new Permission { PermissionName = PERMISSION.ManageEnrollments },

                    // Payment management
                    new Permission { PermissionName = PERMISSION.ViewPayments },
                    new Permission { PermissionName = PERMISSION.ManagePayments },

                    // Logs
                    new Permission { PermissionName = PERMISSION.ViewLogs },

                    // Permission management
                    new Permission { PermissionName = PERMISSION.ViewPermissions },
                    new Permission { PermissionName = PERMISSION.ManagePermissions }
                );
                await context.SaveChangesAsync();
            }

            // Seed RolePermissions
            if (!context.RolePermissions.Any())
            {
                var adminRole = await context.Roles.FirstAsync(r => r.RoleName == "Admin");
                var permissions = await context.Permissions.ToListAsync();

                foreach (var p in permissions)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = adminRole.RoleId,
                        PermissionId = p.PermissionId
                    });
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
