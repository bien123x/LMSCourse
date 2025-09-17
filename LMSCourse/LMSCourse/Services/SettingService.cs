
using AutoMapper;
using LMSCourse.DTOs;
using LMSCourse.DTOs.Setting;
using LMSCourse.DTOs.User;
using LMSCourse.Models;
using LMSCourse.Repositories;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LMSCourse.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repository;
        private readonly IMapper _mapper;

        public SettingService(ISettingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IdentitySettingDto> GetIdentitySettingAsync()
        {
            var identitySetting = await _repository.GetIdentityAsync();

            return _mapper.Map<IdentitySettingDto>(identitySetting);
        }

        public async Task<IdentitySettingDto?> UpdateIdentitySettingAsync(IdentitySettingDto identitySettingDto)
        {
            var identitySetting = await _repository.GetIdentityAsync();

            _mapper.Map(identitySettingDto, identitySetting);

            await _repository.UpdateIdentityAsync(identitySetting);

            return identitySettingDto;
        }

        public async Task<bool> IsPasswordExpiration(DateTime lastChangePassword)
        {
            var identitySetting = await _repository.GetIdentityAsync();
            var policy = identitySetting.Password;

            if (policy.ForceUsersToPeriodicallyChangePassword && policy.PasswordChangePeriodDays > 0)
            {
                var expiredDate = lastChangePassword.AddDays(policy.PasswordChangePeriodDays);
                return DateTime.Now > expiredDate;
            }

            return false;
        }

        public async Task<(bool IsValid, List<string> Errors)> ValidateAsync(string password)
        {
            var identitySetting = await _repository.GetIdentityAsync();
            var policy = identitySetting.Password;
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password cannot be empty.");
                return (false, errors);
            }

            if (password.Length < policy.RequiredLength)
                errors.Add($"Mật khẩu phải có ít nhất {policy.RequiredLength} ký tự.");

            if (policy.RequireDigit && !password.Any(char.IsDigit))
                errors.Add("Mật khẩu phải có ít nhất một số.");

            if (policy.RequireLowercase && !password.Any(char.IsLower))
                errors.Add("Mật khẩu phải có chữ thường.");

            if (policy.RequireUppercase && !password.Any(char.IsUpper))
                errors.Add("Mật khẩu phải có chữ hoa.");

            if (policy.RequireNonAlphanumeric && !Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
                errors.Add("Mật khẩu phải có ký tự đặc biệt.");

            if (policy.RequiredUniqueChars > 1)
            {
                int uniqueCount = password.Distinct().Count();
                if (uniqueCount < policy.RequiredUniqueChars)
                    errors.Add($"Mật khẩu phải có {policy.RequiredUniqueChars} ký tự duy nhất.");
            }

            return (!errors.Any(), errors);
        }

        public async Task<ApiResponse<LockoutAccountDto>> IsLockOutAsync(DateTime? lockoutEndTime, int failedAccessCount)
        {
            var identitySetting = await _repository.GetIdentityAsync();

            var lockout = identitySetting.Lockout;

            if (lockout.AllowedForNewUsers && (lockoutEndTime == null || lockoutEndTime < DateTime.Now))
            {
                if (failedAccessCount > lockout.MaxFailedAccessAttempts)
                {
                    return ApiResponse<LockoutAccountDto>.Ok(new LockoutAccountDto
                    { LockoutDuration = lockout.LockoutDuration, FailedAccessCount = failedAccessCount}, $"Tài khoản đã bị khoá đăng nhập sai quá {lockout.MaxFailedAccessAttempts} lần");
                }
            }

            return ApiResponse<LockoutAccountDto>.Fail($"Tài khoản đã đăng nhập sai {failedAccessCount} lần");

        }

        public async Task<UserSettingDto> GetUserSettingAsync()
        {
            var indentitySetting = await _repository.GetIdentityAsync();
            var userSetting = indentitySetting.User;

            return new UserSettingDto
            {
                IsUserNameUpdateEnabled = userSetting.IsUserNameUpdateEnabled,
                IsEmailUpdateEnabled = userSetting.IsEmailUpdateEnabled,
            };
        }
    }
}
