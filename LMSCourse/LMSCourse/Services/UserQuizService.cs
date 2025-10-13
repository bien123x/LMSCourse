using AutoMapper;
using LMSCourse.DTOs.UserQuiz;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSCourse.Services
{
    public class UserQuizService : IUserQuizService
    {
        private readonly IUserQuizRepository _repo;
        private readonly IQuestionRepository _questionRepo;
        private readonly IQuizRepository _quizRepo;
        private readonly IMapper _mapper;
        public UserQuizService(IUserQuizRepository repo, IMapper mapper, IQuestionRepository questionRepository, IQuizRepository quizRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _questionRepo = questionRepository;
            _quizRepo = quizRepo;
        }
        public async Task<UserQuizViewDto> CreateUserQuizWithUserAnswers(UserQuizDto dto)
        {
            var userQuiz = _mapper.Map<UserQuiz>(dto);

            var countCorrect = 0;

            foreach (var userAnswer in userQuiz.UserAnswers)
            {
                if (await IsCorrectAnswer(userAnswer.QuestionId, userAnswer.AnswerId))
                {
                    countCorrect++;
                }
            }

            var quiz = await _quizRepo.GetByIdAsync(userQuiz.QuizId);

            userQuiz.Score = (int)(((double)countCorrect * quiz.TotalMarks) / quiz.NoOfQuestions);

            await _repo.AddAsync(userQuiz);

            return _mapper.Map<UserQuizViewDto>(userQuiz);
        }

        public async Task<int> CountCorrectAnswer(ICollection<UserAnswer> userAnswers)
        {
            var countCorrect = 0;

            foreach (var userAnswer in userAnswers)
            {
                if (await IsCorrectAnswer(userAnswer.QuestionId, userAnswer.AnswerId))
                {
                    countCorrect++;
                }
            }
            return countCorrect;
        }

        public async Task<IEnumerable<LatestQuizDto>> GetLastestQuizzes(int userId)
        {
            var query = _repo.GetAllQuery();
            var userQuizzes = await query
                .Include(uq => uq.UserAnswers)
                .Where(uq => uq.UserId == userId)
                .OrderByDescending(uq => uq.EndTime)
                .Take(5)
                .ToListAsync();

            var lastestQuizzes = new List<LatestQuizDto>();
            foreach (var userQuiz in userQuizzes)
            {
                var quiz = await _quizRepo.GetByIdAsync(userQuiz.QuizId);
                var countCorrect = await CountCorrectAnswer(userQuiz.UserAnswers);
                lastestQuizzes.Add(new LatestQuizDto
                {
                    CountCorrectAnswer = countCorrect,
                    PercentageScore = countCorrect * 100 / quiz.NoOfQuestions,
                    TitleQuiz = quiz.Title,
                    NoOfQuestions = quiz.NoOfQuestions,
                    UserQuizId = userQuiz.UserQuizId,
                });
            }

            return lastestQuizzes;
        }

        public async Task<bool> IsCorrectAnswer(int questionId, int? answerId)
        {
            var question = await _questionRepo.GetWithAnswer(questionId);

            if (question == null) return false;

            foreach (var answer in question.Answers!)
            {
                if (answer.AnswerId == answerId && answer.IsCorrect) return true;
            }
            return false;
        }

        public async Task<bool> IsPassQuiz(int userQuizId)
        {
            var userQuiz = await _repo.GetWithQuizById(userQuizId);
            

            if (userQuiz != null && userQuiz.Quiz.PassMark <= userQuiz.Score)
                return true;
            return false;
        }
    }
}
