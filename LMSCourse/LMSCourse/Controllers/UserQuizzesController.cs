using LMSCourse.DTOs.UserQuiz;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMSCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserQuizzesController : ControllerBase
    {
        private readonly IUserQuizService _service;
        public UserQuizzesController(IUserQuizService service)
        {
            _service = service;
        }

        [HttpPost("create-user-quiz-with-user-answer")]
        public async Task<IActionResult> CreateUserQuizWithUserAnswers(UserQuizDto dto)
        {
            var userQuizView = await _service.CreateUserQuizWithUserAnswers(dto);

            return Ok(userQuizView);
        }

        [HttpGet("is-pass-quiz/{userQuizId:int}")]
        public async Task<IActionResult> IsPassQuiz(int userQuizId)
        {
            var isPassQuiz = await _service.IsPassQuiz(userQuizId);
            if (isPassQuiz)
            {
                return Ok(new { message = "Chúc mừng! Bạn đã qua bài tập!", passed = isPassQuiz });
            }
            return Ok(new { message = "Bạn chưa qua bài tập!", passed = isPassQuiz });
        }

        [HttpGet("get-lastest-quizzes")]
        public async Task<IActionResult> GetLastestQuizzes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var lastestQuizzes = await _service.GetLastestQuizzes(int.Parse(userId));

            return Ok(lastestQuizzes);
        }
    }
}
