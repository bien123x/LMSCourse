using LMSCourse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineCourseConstants;
using static OnlineCourseConstants.PERMISSION.System;

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

            // Seed Student
            if (!context.Users.Any(u => u.Email == "student@gmail.com"))
            {
                var studentRole = await context.Roles.FirstAsync(r => r.RoleName == "Student");

                var studentUser = new User
                {
                    UserName = "student",
                    Email = "student@gmail.com",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "123")
                };

                context.Users.Add(studentUser);
                await context.SaveChangesAsync();

                // Gán Role cho User
                context.UserRoles.Add(new UserRole
                {
                    UserId = studentUser.UserId,
                    RoleId = studentRole.RoleId,
                });
                await context.SaveChangesAsync();
            }

            // Seed Permissions
            if (!context.Permissions.Any())
            {
                var system = new Permission { PermissionName = "Quản trị hệ thống", PermissionCode = PERMISSION.System.Module };
                context.Permissions.Add(system);
                await context.SaveChangesAsync();

                // User Manager
                var userManagement = new Permission
                {
                    PermissionName = "Quản lý người dùng",
                    PermissionCode = PERMISSION.System.UserManagement.Module,
                    ParentId = system.PermissionId,
                };
                context.Permissions.Add(userManagement);
                await context.SaveChangesAsync();

                // Roles
                var roleGroup = new Permission
                {
                    PermissionName = "Nhóm quyền",
                    PermissionCode = PERMISSION.System.UserManagement.Roles.Module,
                    ParentId = userManagement.PermissionId
                };
                context.Permissions.Add(roleGroup);
                await context.SaveChangesAsync();

                context.Permissions.AddRange(
                    new Permission { PermissionName = "Thay đổi nhóm quyền", PermissionCode = PERMISSION.System.UserManagement.Roles.ChangePermissions, ParentId = roleGroup.PermissionId },
                    new Permission { PermissionName = "Tạo nhóm quyền", PermissionCode = PERMISSION.System.UserManagement.Roles.Create, ParentId = roleGroup.PermissionId },
                    new Permission { PermissionName = "Sửa nhóm quyền", PermissionCode = PERMISSION.System.UserManagement.Roles.Edit, ParentId = roleGroup.PermissionId },
                    new Permission { PermissionName = "Xoá nhóm quyền", PermissionCode = PERMISSION.System.UserManagement.Roles.Delete, ParentId = roleGroup.PermissionId }
                );

                // Users
                var userGroup = new Permission { PermissionName = "Người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.Module, ParentId = userManagement.PermissionId };
                context.Permissions.Add(userGroup); await context.SaveChangesAsync();

                context.Permissions.AddRange(
                    new Permission { PermissionName = "Tạo người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.Create, ParentId = userGroup.PermissionId },
                    new Permission { PermissionName = "Sửa người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.Edit, ParentId = userGroup.PermissionId },
                    new Permission { PermissionName = "Xoá người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.Delete, ParentId = userGroup.PermissionId },
                    new Permission { PermissionName = "Thay đổi nhóm quyền người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.ChangePermissions, ParentId = userGroup.PermissionId },
                    new Permission { PermissionName = "Xem chi tiết người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.ViewDetails, ParentId = userGroup.PermissionId },
                    new Permission { PermissionName = "Thiết đặt mật khẩu người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.ResetPwd, ParentId = userGroup.PermissionId },
                    new Permission { PermissionName = "Khoá tài khoản người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.LockAccount, ParentId = userGroup.PermissionId },
                    new Permission { PermissionName = "Gỡ khoá tài khoản người dùng", PermissionCode = PERMISSION.System.UserManagement.Users.UnLockAccount, ParentId = userGroup.PermissionId }
                );

                var auditLogs = new Permission
                {
                    PermissionName = "Nhật ký hệ thống",
                    PermissionCode = PERMISSION.System.AuditLogs.Module,
                    ParentId = system.PermissionId,
                };
                context.Permissions.Add(auditLogs);
                await context.SaveChangesAsync();

                context.Permissions.AddRange(
                    new Permission { PermissionName = "Xuất nhật ký", PermissionCode = PERMISSION.System.AuditLogs.Export, ParentId = auditLogs.PermissionId }
                );

                // Cài đặt
                var settings = new Permission { PermissionName = "Cài đặt", PermissionCode = PERMISSION.System.Settings.Module, ParentId = system.PermissionId };
                context.Permissions.Add(settings);
                await context.SaveChangesAsync();

                context.Permissions.AddRange(
                    new Permission { PermissionName = "Cập nhật cài đặt", PermissionCode = PERMISSION.System.Settings.Edit, ParentId = settings.PermissionId }
                );

                var student = new Permission
                {
                    PermissionName = "Quản lý học sinh",
                    PermissionCode = PERMISSION.Students.Module,
                };
                context.Permissions.Add(student);
                await context.SaveChangesAsync();

                // Course
                var courseStudent = new Permission { PermissionName = "Khoá học", PermissionCode = PERMISSION.Students.Courses.Module, ParentId = student.PermissionId };
                context.Permissions.Add(courseStudent);
                await context.SaveChangesAsync();

                var courseViewDetail = new Permission { PermissionName = "Xem chi tiết khoá học", PermissionCode = PERMISSION.Students.Courses.ViewDetails.Module, ParentId = courseStudent.PermissionId };
                context.Permissions.Add(courseViewDetail);
                await context.SaveChangesAsync();

                context.Permissions.Add(
                    new Permission { PermissionName = "Thêm vào giỏ hàng", PermissionCode = PERMISSION.Students.Courses.ViewDetails.AddToCart, ParentId = courseViewDetail.PermissionId }
                );

                var payment = new Permission { PermissionName = "Thanh toán giỏ hàng", PermissionCode = PERMISSION.Students.Payments.Module, ParentId = student.PermissionId };
                context.Permissions.Add(payment);
                await context.SaveChangesAsync();

                context.Permissions.AddRange(
                    new Permission { PermissionName = "Thanh toán", PermissionCode = PERMISSION.Students.Payments.Checkout, ParentId = payment.PermissionId },
                    new Permission { PermissionName = "Xem lịch sử thanh toán", PermissionCode = PERMISSION.Students.Payments.ViewHistory, ParentId = payment.PermissionId }
                );

                var enrollment = new Permission { PermissionName = "Ghi danh khoá học", PermissionCode = PERMISSION.Students.Enrollments.Module, ParentId = student.PermissionId };
                context.Permissions.Add(enrollment);
                await context.SaveChangesAsync();

                var enrollmentDetail = new Permission { PermissionName = "Xem khoá học", PermissionCode = PERMISSION.Students.Enrollments.ViewDetails.Module, ParentId = enrollment.PermissionId };
                context.Permissions.Add(enrollmentDetail);
                await context.SaveChangesAsync();

                context.Permissions.AddRange(
                    new Permission { PermissionName = "Xem bài học", PermissionCode = PERMISSION.Students.Enrollments.ViewDetails.WatchVideo, ParentId = enrollmentDetail.PermissionId },
                    new Permission { PermissionName = "Làm bài tập", PermissionCode = PERMISSION.Students.Enrollments.ViewDetails.DoQuiz, ParentId = enrollmentDetail.PermissionId }
                );

                var certificate = new Permission { PermissionName = "Chứng chỉ", PermissionCode = PERMISSION.Students.Certificates.Module, ParentId = student.PermissionId };
                context.Permissions.Add(certificate);
                await context.SaveChangesAsync();

                context.Permissions.AddRange(
                    new Permission { PermissionName = "Xem chi tiết chứng chỉ", PermissionCode = PERMISSION.Students.Certificates.ViewDetails, ParentId = certificate.PermissionId }
                );

                await context.SaveChangesAsync();
            }

            if (!context.UserPermissions.Any(up => up.User.UserName == "admin"))
            {
                var permissions = await context.Permissions.ToListAsync();

                var admin = await context.Users.FirstOrDefaultAsync(u => u.UserName == "admin");

                var userPermissions = new List<UserPermission>();
                foreach (var permission in permissions)
                {
                    userPermissions.Add(new UserPermission
                    {
                        UserId = admin!.UserId,
                        PermissionId = permission.PermissionId,
                    });
                }
                await context.UserPermissions.AddRangeAsync(userPermissions);

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
                            //rolePermissions = permissions.Where(p =>
                            //    p.PermissionCode == PERMISSION.System ||
                            //    p.PermissionCode == PERMISSION.CreateCourses ||
                            //    p.PermissionCode == PERMISSION.EditCourses ||
                            //    p.PermissionCode == PERMISSION.DeleteCourses ||

                            //    p.PermissionCode == PERMISSION.ViewLessons ||
                            //    p.PermissionCode == PERMISSION.CreateLessons ||
                            //    p.PermissionCode == PERMISSION.EditLessons ||
                            //    p.PermissionCode == PERMISSION.DeleteLessons ||

                            //    p.PermissionCode == PERMISSION.ViewEnrollments ||
                            //    p.PermissionCode == PERMISSION.ManageEnrollments
                            //).ToList();
                            rolePermissions = new List<Permission>();
                            break;

                        case "Student":
                            // Student chỉ có quyền xem thông tin
                            //rolePermissions = permissions.Where(p =>
                            //    p.PermissionName == PERMISSION.ViewCourses ||
                            //    p.PermissionName == PERMISSION.ViewLessons ||
                            //    p.PermissionName == PERMISSION.ViewEnrollments ||
                            //    p.PermissionName == PERMISSION.ViewPayments
                            //).ToList();
                            rolePermissions = new List<Permission>();
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
                    new Category { Name = "Tài chính" },
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

            // Seeding Courses
            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course
                    {
                        Title = "Khóa học C# Cơ bản",
                        IsPublic = true,
                        MaxStudents = 100,
                        ShortDescription = "Lập trình C# từ cơ bản đến nâng cao.",
                        Description = "Khóa học cung cấp kiến thức C# chi tiết với nhiều ví dụ thực tế.",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TeacherId = 2,          // Id giáo viên có sẵn
                        CategoryId = 1,         // Id danh mục có sẵn
                        LevelId = 1,            // Beginner
                        LanguageId = 1,         // Tiếng Việt
                        ThumbnailUrl = "/images/courses/csharp.jpg",
                        VideoType = "YouTube",
                        VideoUrl = "https://www.youtube.com/watch?v=xxxxxx",
                        IsFree = false,
                        Price = 1000000,
                        HasDiscount = true,
                        DiscountPrice = 700000,
                        IsLifetime = true
                    },
                    new Course
                    {
                        Title = "Khóa học ASP.NET Core MVC",
                        IsPublic = true,
                        MaxStudents = 200,
                        ShortDescription = "Xây dựng ứng dụng web với ASP.NET Core MVC.",
                        Description = "Học cách phát triển ứng dụng web chuyên nghiệp bằng ASP.NET Core MVC.",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TeacherId = 2,
                        CategoryId = 2,
                        LevelId = 2, // Intermediate
                        LanguageId = 1,
                        ThumbnailUrl = "/images/courses/aspnet.jpg",
                        VideoType = "YouTube",
                        VideoUrl = "https://www.youtube.com/watch?v=yyyyyy",
                        IsFree = false,
                        Price = 1500000,
                        HasDiscount = false,
                        DiscountPrice = null,
                        IsLifetime = false,
                        DurationInMonths = 12
                    }
                );

                context.SaveChanges();
            }

            if (!context.FaqGroups.Any())
            {
                var courses = context.Courses.ToList();
                foreach (var course in courses)
                {
                    context.FaqGroups.AddRange(
                        new FaqGroup
                        {
                            Title = "FAQ " + course.Title,
                            CourseId = course.CourseId,
                        },
                        new FaqGroup
                        {
                            Title = "FAQ2 " + course.Title,
                            CourseId = course.CourseId,
                        }
                    );
                }
                context.SaveChanges();
            }

            if (!context.FaqItems.Any())
            {
                var faqGroups = context.FaqGroups.ToList();
                foreach (var faqGroup in faqGroups)
                {
                    context.FaqItems.AddRange(
                        new FaqItem
                        {
                            Question = "Làm thế nào để đăng ký khóa học?",
                            Answer = "Bạn có thể đăng ký khóa học bằng cách nhấn nút Đăng ký trên trang chi tiết khóa học.",
                            FaqGroupId = faqGroup.FaqGroupId
                        },
                        new FaqItem
                        {
                            Question = "Tôi có thể hủy khóa học sau khi đăng ký không?",
                            Answer = "Bạn có thể hủy khóa học trong vòng 7 ngày kể từ ngày đăng ký.",
                            FaqGroupId = faqGroup.FaqGroupId
                        }
                    );
                }
                context.SaveChanges();
            }

            if (!context.CourseTopics.Any())
            {
                var courses = context.Courses.ToList();
                foreach (var course in courses)
                {
                    context.CourseTopics.AddRange(
                        new CourseTopic
                        {
                            Title = "Chủ đề 1",
                            CourseId = course.CourseId,
                        },
                        new CourseTopic
                        {
                            Title = "Chủ đề 2",
                            CourseId = course.CourseId
                        }
                    );
                }
                context.SaveChanges();
            }

            if (!context.Lessons.Any())
            {
                var courseTopics = context.CourseTopics.ToList();
                foreach (var courseTopic in courseTopics)
                {
                    context.Lessons.AddRange(
                        new Lesson
                        {
                            Title = "Bài học 1",
                            LessonContent = "https://www.youtube.com/embed/1trvO6dqQUI",
                            Description = "Mô tả bài học",
                            IsFreeOrPremium = true,
                            CourseTopicId = courseTopic.CourseTopicId,
                        },
                        new Lesson
                        {
                            Title = "Bài học 2",
                            LessonContent = "https://www.youtube.com/embed/1trvO6dqQUI",
                            Description = "Mô tả bài học",
                            IsFreeOrPremium = false,
                            CourseTopicId = courseTopic.CourseTopicId,
                        }
                    );
                }
                context.SaveChanges();
            }

        }
    }
}
