using LMSCourse.DTOs.Question;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionSevice _service;
        public QuestionsController(IQuestionSevice service)
        {
            _service = service;
        }

        [HttpPost("create-with-answers")]
        public async Task<IActionResult> CreateWithAnswers(QuestionDto dto)
        {
            var questionView = await _service.CreateQuestionWithAnswerAsync(dto);

            return Ok(questionView);
        }

        [HttpGet("get-questions-with-answers/{quizId:int}")]
        public async Task<IActionResult> GetQuestionsWithAnswerByQuizId(int quizId)
        {
            var questionsView = await _service.GetQuestionsWithAnswerByQuizId(quizId);
            return Ok(questionsView);
        }
    }
}
