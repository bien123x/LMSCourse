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
        private readonly ISettingService _settingsService;

        public AuthController(TokenService tokenService, IUserService userService, ISettingService settingsService)
        {
            this._tokenService = tokenService;
            this._userService = userService;
            _settingsService = settingsService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.GetUserByUserNameOrEmailAsync(dto.UserNameOrEmail);

            if (user == null)
            {
                return Unauthorized();
            }
            if (!user.IsActive)
                return BadRequest("Tài khoản đã bị khoá!");
            if (user.LockoutEndTime != null && user.LockoutEndTime > DateTime.Now)
                return BadRequest($"Tài khoản đã bị khoá đến {user.LockoutEndTime}!");
            else if (user.LockoutEndTime != null)
            {
                await _userService.SetLockEndTimeAsync(user.UserId, -1);
                await _userService.ResetFailAccessCount(user.UserId);

            }


            if (!_userService.VerifyPassword(user, dto.Password))
            {
                var result = await _settingsService.IsLockOutAsync(user.LockoutEndTime, user.FailedAccessCount + 1);
                // Lock
                if (result.Success)
                {
                    // Set LockEndTime
                    await _userService.SetLockEndTimeAsync(user.UserId, result.Data.LockoutDuration);
                    return Ok(result);
                }
                // Update Count Access Fail
                await _userService.IncreaseFailAccessCount(user.UserId);
                return BadRequest(result);
            }
            else
            {
                await _userService.ResetFailAccessCount(user.UserId);
            }

            // Check force periodically change password
            if (await _settingsService.IsPasswordExpiration(user.PasswordUpdateTime))
                return Ok(new { requirePasswordChange = true, userId = user.UserId });

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
            var (isValid, errors) = await _settingsService.ValidateAsync(dto.PasswordHash);

            if (!isValid)
                return BadRequest(new { Errors = errors });

            var user = await _userService.RegisterUserAsync(dto);


            if (user != null)
            {
                return Ok("");
            }
            return BadRequest("Tên đăng nhập/Email đã tồn tại!");
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var user = await _userService.VerifyEmailByToken(token);

            if (user == null)
                return BadRequest("Không hợp lệ.");

            return Ok("Xác thực Email thành công.");
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
