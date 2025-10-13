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
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserQuiz> UserQuizzes { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CertificateTemplate> CertificateTemplates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    fk.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            // User → Enrollment (user is student)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Enrollments)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // (user is teacher)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Courses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course → Enrollment
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Payment
            modelBuilder.Entity<User>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course → Payment
            modelBuilder.Entity<Course>()
                .HasMany(c => c.PaymentDetails)
                .WithOne(p => p.Course)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            //Question
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>()
                .HasMany(q => q.UserAnswers)
                .WithOne(ua => ua.Question)
                .OnDelete(DeleteBehavior.Restrict);

            // Certificate
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.Enrollment)
                .WithOne(e => e.Certificate)
                .OnDelete(DeleteBehavior.Restrict);

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
