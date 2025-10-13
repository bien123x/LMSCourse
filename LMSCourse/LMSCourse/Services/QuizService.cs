using AutoMapper;
using LMSCourse.DTOs.Quiz;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;

namespace LMSCourse.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _repo;
        private readonly IMapper _mapper;
        public QuizService(IQuizRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<QuizViewDto?> CreateQuiz(QuizDto dto)
        {
            var quiz = _mapper.Map<Quiz>(dto);
            await _repo.AddAsync(quiz);
            return _mapper.Map<QuizViewDto>(quiz);
        }

        public async Task<QuizViewDto?> GetQuizById(int quizId)
        {
            var quiz = await _repo.GetByIdAsync(quizId);
            if (quiz == null) return null;

            return _mapper.Map<QuizViewDto>(quiz);
        }

        public async Task<IEnumerable<QuizViewDto>?> GetQuizzesByCourseId(int courseId)
        {
            var quizzes = await _repo.GetQuizzesByCourseId(courseId);
            if (quizzes == null) return null;
            var viewQuizzes = new List<QuizViewDto>();
            foreach (var quiz in quizzes)
            {
                viewQuizzes.Add(new QuizViewDto
                {
                    QuizId = quiz.QuizId,
                    CourseId = courseId,
                    Title = quiz.Title,
                    NoOfQuestions = quiz.NoOfQuestions,
                    TotalMarks = quiz.TotalMarks,
                    PassMark = quiz.PassMark,
                    Duration = quiz.Duration,
                });
            }

            return viewQuizzes;
        }
    }
}
