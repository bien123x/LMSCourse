using Azure.Core;
using LMSCourse.DTOs.Token;
using LMSCourse.DTOs.User;
using LMSCourse.Services;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMSCourse.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(TokenService tokenService, IUserService userService)
        {
            this._tokenService = tokenService;
            this._userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.GetUserByUserNameOrEmailAsync(dto.UserNameOrEmail);

            if (user == null) {
                return Unauthorized();
            }

            if (!_userService.VerifyPassword(user, dto.Password))
                return Unauthorized("Mật khẩu không đúng!");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = await _userService.RegisterUserAsync(dto);

            if (user != null) {
                return CreatedAtAction(actionName: nameof(UserController.GetUserById), controllerName: "User", routeValues: new { id = user.UserId }, value: user);
            }
            return BadRequest("Tên đăng nhập/Email đã tồn tại!");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            try
            {
                var tokens = await _tokenService.RefreshAsync(dto.RefreshToken);
                return Ok(new
                {
                    AccessToken = tokens.accessToken,
                    RefreshToken = tokens.refreshToken
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            // Lấy userId từ claim
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.UserName,
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userService.GetUserByUserNameOrEmailAsync(dto.Email);

            if (user == null)
                return NotFound("User không tồn tại");

            var token = _tokenService.GenerateResetPasswordToken(user.Email);

            var resetLink = $"http://localhost:4200/auth/reset-password?token={Uri.EscapeDataString(token)}";

            return Ok(new { Message = "Reset password link sent to your email", ResetLink = resetLink });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                var principal = _tokenService.ValidateResetPasswordToken(dto.Token);
                var email = principal.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrEmpty(email))
                    return BadRequest("Invalid token");

                var user = await _userService.GetUserByUserNameOrEmailAsync(email);
                if (user == null) return NotFound("User not found");

                user.PasswordHash = _userService.HashPasswordUser(user, dto.NewPassword);
                await _userService.UpdateUserAsync(user);

                return Ok(new { Message = "Password reset successfully" });
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch
            {
                return BadRequest("Invalid or expired token");
            }
        }
    }
}
