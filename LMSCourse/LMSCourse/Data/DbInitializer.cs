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

            // Seed Teacher User
            if (!context.Users.Any(u => u.Email == "22a1001d0032@students.hou.edu.vn"))
            {
                var teacherRole = await context.Roles.FirstAsync(r => r.RoleName == "Teacher");

                var teacherUser = new User
                {
                    UserName = "teacher",
                    Email = "22a1001d0032@students.hou.edu.vn",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123")
                };

                context.Users.Add(teacherUser);
                await context.SaveChangesAsync();

                // Gán Role cho User
                context.UserRoles.Add(new UserRole
                {
                    UserId = teacherUser.UserId,
                    RoleId = teacherRole.RoleId,
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
                var roles = await context.Roles
                    .Where(r => r.RoleName == "Admin" || r.RoleName == "Teacher" || r.RoleName == "Student")
                    .ToListAsync();

                var permissions = await context.Permissions.ToListAsync();

                foreach (var role in roles)
                {
                    List<Permission> rolePermissions;

                    switch (role.RoleName)
                    {
                        case "Admin":
                            // Admin có tất cả permissions
                            rolePermissions = permissions;
                            break;

                        case "Teacher":
                            // Teacher chỉ có quản lý Course, Lesson, Enrollment
                            rolePermissions = permissions.Where(p =>
                                p.PermissionName == PERMISSION.ViewCourses ||
                                p.PermissionName == PERMISSION.CreateCourses ||
                                p.PermissionName == PERMISSION.EditCourses ||
                                p.PermissionName == PERMISSION.DeleteCourses ||

                                p.PermissionName == PERMISSION.ViewLessons ||
                                p.PermissionName == PERMISSION.CreateLessons ||
                                p.PermissionName == PERMISSION.EditLessons ||
                                p.PermissionName == PERMISSION.DeleteLessons ||

                                p.PermissionName == PERMISSION.ViewEnrollments ||
                                p.PermissionName == PERMISSION.ManageEnrollments
                            ).ToList();
                            break;

                        case "Student":
                            // Student chỉ có quyền xem thông tin
                            rolePermissions = permissions.Where(p =>
                                p.PermissionName == PERMISSION.ViewCourses ||
                                p.PermissionName == PERMISSION.ViewLessons ||
                                p.PermissionName == PERMISSION.ViewEnrollments ||
                                p.PermissionName == PERMISSION.ViewPayments
                            ).ToList();
                            break;

                        default:
                            rolePermissions = new List<Permission>();
                            break;
                    }

                    foreach (var permission in rolePermissions)
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = role.RoleId,
                            PermissionId = permission.PermissionId
                        });
                    }
                }

                await context.SaveChangesAsync();
            }


            if (!context.IdentitySettings.Any())
            {
                await context.IdentitySettings.AddAsync(new IdentitySetting
                {
                    Password = new PasswordSetting
                    {
                        RequiredLength = 3,
                        RequiredUniqueChars = 1,
                        RequireNonAlphanumeric = false,
                        RequireLowercase = false,
                        RequireUppercase = false,
                        RequireDigit = false,
                        ForceUsersToPeriodicallyChangePassword = false,
                        PasswordChangePeriodDays = 0,
                    },
                    Lockout = new LockoutSetting
                    {
                        AllowedForNewUsers = true,
                        LockoutDuration = 300,
                        MaxFailedAccessAttempts = 5
                    },
                    SignIn = new SignInSetting
                    {
                        RequireConfirmedEmail = false,
                        RequireEmailVerificationToRegister = false,
                        EnablePhoneNumberConfirmation = true,
                        RequireConfirmedPhoneNumber = false,
                    },
                    User = new UserSetting
                    {
                        IsUserNameUpdateEnabled = true,
                        IsEmailUpdateEnabled = true,
                    }
                });

                await context.SaveChangesAsync();
            }

            if (!context.Categories.Any())
            {
                await context.Categories.AddRangeAsync(
                    new Category { Name = "Quản trị" },
                    new Category { Name = "CNTT & Phần mềm" },
                    new Category { Name = "Tiếp thị" },
                    new Category {Name = "Tài chính" }, 
                    new Category { Name = "Năng suất" }
                );
                await context.SaveChangesAsync();
            }

            // Languages
            if (!context.Languages.Any())
            {
                await context.Languages.AddRangeAsync(
                    new Language { Name = "Tiếng Việt" },
                    new Language { Name = "Tiếng Anh" },
                    new Language { Name = "Tiếng Pháp" },
                    new Language { Name = "Tiếng Nhật" }
                );
                await context.SaveChangesAsync();
            }

            // Levels
            if (!context.Levels.Any())
            {
                await context.Levels.AddRangeAsync(
                    new Level { Name = "Cơ bản" },
                    new Level { Name = "Trung cấp" },
                    new Level { Name = "Nâng cao" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
