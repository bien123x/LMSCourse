using AutoMapper;
using LMSCourse.DTOs.Question;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;

namespace LMSCourse.Services
{
    public class QuestionService : IQuestionSevice
    {
        private readonly IQuestionRepository _repo;
        private readonly IMapper _mapper;
        public QuestionService(IQuestionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<QuestionViewDto> CreateQuestionWithAnswerAsync(QuestionDto dto)
        {
            var question = _mapper.Map<Question>(dto);
            await _repo.AddAsync(question);
            return _mapper.Map<QuestionViewDto>(question);
        }

        public async Task<IEnumerable<QuestionViewDto>> GetQuestionsWithAnswerByQuizId(int quizId)
        {
            var questions = await _repo.GetQuestionsWithAnswerByQuizId(quizId);

            return _mapper.Map<IEnumerable<QuestionViewDto>>(questions);
        }

        
    }
}
