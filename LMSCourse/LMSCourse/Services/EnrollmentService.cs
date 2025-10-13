using AutoMapper;
using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.Enrollment;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repo;
        private readonly IQuizRepository _quizRepo;
        private readonly IUserQuizRepository _userQuizRepo;
        private readonly IMapper _mapper;
        public EnrollmentService(IEnrollmentRepository repo, IMapper mapper, IQuizRepository quizRepository, IUserQuizRepository userQuizRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _quizRepo = quizRepository;
            _userQuizRepo = userQuizRepo;
        }

        public async Task<DashboardEnrollmentCourseDto?> GetDashboardEnrollmentCourseByIdAsync(int userId)
        {
            var query = _repo.GetAllQueryable(userId);

            var latestEnrollments = await query.OrderBy(e => e.EnrollDate)
                .Take(3)
                .ToListAsync();
            return new DashboardEnrollmentCourseDto
            {
                CountActiveCourse = query.Where(e => e.Status == "Active").Count(),
                CountCompleteCourse = query.Where(e => e.Status == "Completed").Count(),
                CountEnrolledCourse = query.Count(),
                RecentEnrolledCourses = _mapper.Map<List<EnrollmentDto>>(latestEnrollments)
            };
        }

        public async Task<EnrollmentDto?> GetEnrollmentByIdAsync(int courseId, int userId)
        {
            var enrollment = await _repo.GetWithCourseByIdAsync(courseId, userId);
            if (enrollment == null)
            {
                return null;
            }
            return _mapper.Map<EnrollmentDto>(enrollment);
        }

        public async Task<bool> IsCompleteCourse(int courseId, int userId)
        {
            var enrollment = await _repo.GetWithCourseByIdAsync(courseId, userId);
            var quizzes = await _quizRepo.GetQuizzesByCourseId(courseId);
            var query = _userQuizRepo.GetAllQuery();
            var userQuizzes = await query.Where(uq => uq.UserId == userId).ToListAsync();
            foreach (var quiz in quizzes)
            {
                if (userQuizzes.Select(uq => uq.QuizId).Contains(quiz.QuizId))
                {
                    var tempUserQuiz = userQuizzes.Where(uq => uq.QuizId == quiz.QuizId).MaxBy(uq => uq.Score);
                    if (tempUserQuiz.Score < quiz.PassMark)
                    {
                        return false;
                    }
                } else
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> IsExistCertificate(int enrollmentId)
        {
            var enrollment = await _repo.GetWithCertificateByIdAsync(enrollmentId);
            if (enrollment !=null &&  enrollment.Certificate != null) 
                return true;
            return false;
        }

        public async Task<EnrollmentDto> UpdateStatus(int courseId, int userId)
        {
            var enrollment = await _repo.GetWithCourseByIdAsync(courseId, userId);

            var isComplete = await IsCompleteCourse(courseId, userId);
            if (isComplete)
            {
                enrollment.Status = "Completed";
            } else
            {
                enrollment.Status = "Active";
            }
            await _repo.UpdateAsync(enrollment);
            return _mapper.Map<EnrollmentDto>(enrollment);
        }

    }
}
