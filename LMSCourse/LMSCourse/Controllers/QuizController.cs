using LMSCourse.DTOs.Quiz;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _service;
        public QuizController(IQuizService service)
        {
            _service = service;
        }

        [HttpGet("get-quizzes-by-courseId/{courseId:int}")]
        public async Task<IActionResult> GetQuizzesByCourseId(int courseId)
        {
            var quizzes = await _service.GetQuizzesByCourseId(courseId);

            if (quizzes == null) return NotFound("Không tồn tại quiz!");

            return Ok(quizzes);
        }

        [HttpPost("create-quiz")]
        public async Task<IActionResult> CreateQuiz(QuizDto dto) 
        {
            var quizView = await _service.CreateQuiz(dto);

            return Ok(quizView);
        }

        [HttpGet("get-quiz-by-id/{quizId:int}")]
        public async Task<IActionResult> GetQuizById(int quizId)
        {
            var quizView = await _service.GetQuizById(quizId);
            if (quizView == null) return NotFound("Không tồn tại bài tập!");
            return Ok(quizView);
        }
    }
}
