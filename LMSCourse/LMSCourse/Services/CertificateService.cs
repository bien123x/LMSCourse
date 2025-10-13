using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using LMSCourse.DTOs.Certificate;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LMSCourse.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICertificateRepository _certificateRepo;
        private readonly ICertificateTemplateRepository _certificateTemplateRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IUserRepository _userRepo;
        private readonly IQuizRepository _quizRepo;
        private readonly IUserQuizRepository _userQuizRepo;
        public CertificateService(IWebHostEnvironment env, ICertificateTemplateRepository certificateTemplateRepo, ICertificateRepository certificateRepo, IUserRepository userRepository, ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepo, IQuizRepository quizRepository, IUserQuizRepository userQuizRepo)
        {
            _env = env;
            _certificateTemplateRepo = certificateTemplateRepo;
            _certificateRepo = certificateRepo;
            _userRepo = userRepository;
            _courseRepo = courseRepository;
            _enrollmentRepo = enrollmentRepo;
            _quizRepo = quizRepository;
            _userQuizRepo = userQuizRepo;
        }

        public async Task CreateCertificate(CertificateDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.userId);
            var teacher = await _userRepo.GetByIdAsync(dto.teacherId);
            var course = await _courseRepo.GetByIdAsync(dto.courseId);

            var certificateTemplate = await _certificateTemplateRepo.GetStatusIsActiveByTeacherId(dto.teacherId);

            var certificateCode = Guid.NewGuid().ToString("N");
            var issueDate = DateTime.UtcNow;
            var certificateUrl = GenerateCertificate(user.Name + " " + user.Surname, course.Title, certificateTemplate.TemplateUrl, issueDate, certificateCode);
            var enrollment = await _enrollmentRepo.GetWithCourseByIdAsync(course.CourseId, user.UserId);

            var certificate = new Certificate
            {
                CertificateName = course.Title,
                CertificateUrl = certificateUrl,
                CertificateTemplateId = certificateTemplate.CertificateTemplateId,
                IssueDate = issueDate,
                Status = "Issued",
                EnrollmentId = enrollment.EnrollmentId,
            };
            await _certificateRepo.AddAsync(certificate);
        }

        public async Task<IEnumerable<CertificateViewDto>?> GetCertificatesByUserId(int userId)
        {
            var query = _certificateRepo.GetAllQuery();
            var certificates = query.Include(c => c.Enrollment).ToList();
            var cerView = new List<CertificateViewDto>();
            foreach (var certificate in certificates)
            {
                if (certificate.Enrollment.UserId == userId)
                {
                    cerView.Add(new CertificateViewDto
                    {
                        CertificateName = certificate.CertificateName,
                        CertificateUrl = certificate.CertificateUrl,
                        CertificateId = certificate.CertificateId,
                        IssueDate = certificate.IssueDate,
                        Marks = await _userQuizRepo.GetMarkByCourseId(certificate.Enrollment.CourseId, userId),
                        OutOf = await _quizRepo.GetTotalMarksByCourseId(certificate.Enrollment.CourseId)
                    });
                }
            }
            return cerView;
        }

        public string GenerateCertificate(string studentName, string certificateName, string templateUrl, DateTime completeDate, string certificateCode)
        {
            var templatePath = System.IO.Path.Combine(_env.WebRootPath, "templates", templateUrl);
            var outputDir = System.IO.Path.Combine(_env.WebRootPath, "certificates");

            Directory.CreateDirectory(outputDir);

            var outputPath = System.IO.Path.Combine(outputDir, $"{certificateCode}.pdf");

            var writer = new PdfWriter(outputPath);
            var pdf = new PdfDocument(writer);
            var pageSize = PageSize.A4.Rotate(); // khổ ngang đẹp hơn cho certificate
            var document = new Document(pdf, pageSize);
            document.SetMargins(0, 0, 0, 0);

            // 1️⃣ Thêm ảnh nền full trang
            var bg = new Image(ImageDataFactory.Create(templatePath));
            bg.ScaleToFit(pageSize.GetWidth(), pageSize.GetHeight());
            bg.SetFixedPosition(0, 0);
            document.Add(bg);

            // 2️⃣ Nạp font tiếng Việt
            var fontPath = System.IO.Path.Combine(_env.WebRootPath, "fonts", "Inter-VariableFont_opsz,wght.ttf");
            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);

            // 3️⃣ Vẽ chữ bằng Canvas (canh tọa độ chính xác)
            var canvas = new iText.Layout.Canvas(
                new iText.Kernel.Pdf.Canvas.PdfCanvas(pdf.GetFirstPage()),
                pageSize
            );

            // 🎓 Tiêu đề
            canvas.ShowTextAligned(
                new Paragraph("CHỨNG NHẬN HOÀN THÀNH")
                    .SetFont(font)
                    .SetBold()
                    .SetFontSize(32)
                    .SetFontColor(ColorConstants.BLACK),
                pageSize.GetWidth() / 2,
                470,
                TextAlignment.CENTER
            );

            // 👤 Tên học viên
            canvas.ShowTextAligned(
                new Paragraph(studentName.ToUpper())
                    .SetFont(font)
                    .SetBold()
                    .SetFontSize(26)
                    .SetFontColor(ColorConstants.BLUE),
                pageSize.GetWidth() / 2,
                400,
                TextAlignment.CENTER
            );

            // 📘 Tên khóa học
            canvas.ShowTextAligned(
                new Paragraph($"Đã hoàn thành khóa học: {certificateName}")
                    .SetFont(font)
                    .SetFontSize(20)
                    .SetFontColor(ColorConstants.DARK_GRAY),
                pageSize.GetWidth() / 2,
                350,
                TextAlignment.CENTER
            );

            // 📅 Ngày hoàn thành
            canvas.ShowTextAligned(
                new Paragraph($"Ngày hoàn thành: {completeDate:dd/MM/yyyy}")
                    .SetFont(font)
                    .SetFontSize(16),
                pageSize.GetWidth() / 2,
                300,
                TextAlignment.CENTER
            );

            // 🔖 Mã chứng chỉ
            canvas.ShowTextAligned(
                new Paragraph($"Mã chứng chỉ: {certificateCode}")
                    .SetFont(font)
                    .SetFontSize(14)
                    .SetFontColor(ColorConstants.GRAY),
                pageSize.GetWidth() / 2,
                270,
                TextAlignment.CENTER
            );

            document.Close();

            return $"/certificates/{certificateCode}.pdf";
        }
    }
}
