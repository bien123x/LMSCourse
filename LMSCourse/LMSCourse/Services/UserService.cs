using AutoMapper;
using LMSCourse.DTOs;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.DTOs.User;
using LMSCourse.Models;
using LMSCourse.Repositories;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace LMSCourse.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<ViewUserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return null;

            return _mapper.Map<ViewUserDto>(user);
        }

        public async Task<List<string>> GetRolesNameByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserWithRolesAsync(userId);
            if (user == null)
                return new List<string>(); // hoặc throw exception nếu muốn

            return user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        }

        public async Task<List<string>> GetPermissionsNameByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserWithRolesAndPermissionsAsync(userId);

            if (user == null)
                return new List<string>();

            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.PermissionName)
                .ToList();

            return permissions;
        }

        public bool VerifyPassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<User?> GetUserByUserNameOrEmailAsync(string userOrEmail)
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(userOrEmail);

            if (user == null)
                return null;

            return user;
        }

        public async Task<ApiResponse<User>> RegisterUserAsync(RegisterDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.PasswordHash);

            user.TokenEmail = Guid.NewGuid().ToString();
            user.TokenEmailExpires = DateTime.UtcNow.AddHours(24);
            //

            if (await _userRepository.CheckExistUserNameOrEmail(dto.UserName))
            {
                return ApiResponse<User>.Fail("Tên đăng nhập đã tồn tại");
            }
            if (await _userRepository.CheckExistUserNameOrEmail(dto.Email))
            {
                return ApiResponse<User>.Fail("Email đã tồn tại");
            }
            await _userRepository.AddAsync(user);
            return ApiResponse<User>.Ok(user, "Người dùng hợp lệ");
        }

        public string HashPasswordUser(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task<IEnumerable<ViewUserDto>> GetAllViewUser()
        {
            var users = await _userRepository.GetAllWithRolesAsync();

            return _mapper.Map<IEnumerable<ViewUserDto>>(users);
        }

        public async Task<ViewUserDto> AddUserAsync(UserDto userDto, int addUserId)
        {
            var user = _mapper.Map<User>(userDto);
            var addUser = await _userRepository.GetByIdAsync(addUserId);
            if (await _userRepository.IsExistUserNameOrEmail(user.UserName, user.Email) || addUser == null)
                return null;
            user.PasswordHash = HashPasswordUser(user, userDto.PasswordHash);

            user.UserRoles = new List<UserRole>();
            foreach (var roleName in userDto.Roles)
            {
                var role = await _userRepository.GetRoleByRoleName(roleName);
                user.UserRoles.Add(new UserRole
                {
                    User = user,
                    RoleId = role.RoleId
                });
            }
            user.CreationTime = DateTime.UtcNow;
            user.ModificationTime = DateTime.UtcNow;
            user.CreatedBy = addUser.Name;
            await _userRepository.AddAsync(user);
            return _mapper.Map<ViewUserDto>(user);
        }

        public async Task<ViewUserDto> EditUserDto(int userId, EditUserDto editUserDto, int editUserId)
        {
            var user = await _userRepository.GetUserWithRolesAsync(userId);
            var editUser = await _userRepository.GetByIdAsync(editUserId);


            if (user == null || editUser == null)
            {
                return null;
            }

            _mapper.Map(editUserDto, user);

            if (await _userRepository.IsExistUserNameOrEmail(user.UserName, user.Email, user.UserId))
                return null;

            //if (editUserDto.Roles != null && editUserDto.Roles.Any())
            //{
            user.UserRoles.Clear();
            foreach (var roleName in editUserDto.Roles)
            {
                var role = await _userRepository.GetRoleByRoleName(roleName);
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId
                });
            }
            //}
            user.ModificationTime = DateTime.UtcNow;
            user.ModifiedBy = editUser.Name;
            await _userRepository.UpdateAsync(user);

            var viewUserDto = _mapper.Map<ViewUserDto>(user);

            return viewUserDto;
        }
        public async Task<List<string>> GetRolesName()
        {
            var roles = await _userRepository.GetAllRoles();
            var rolesName = new List<string>();
            foreach (var role in roles)
            {
                rolesName.Add(role.RoleName);
            }
            return rolesName;
        }

        public async Task<List<string>> GetUserPermissionsNameById(int userId)
        {
            var user = await _userRepository.GetWithUserPermissions(userId);
            if (user == null)
            {
                return new List<string>();
            }
            var permissions = new List<string>();
            foreach (var userPermission in user.UserPermissions)
            {
                permissions.Add(userPermission.Permission.PermissionName);
            }

            return permissions;
        }

        public async Task<List<string>> UpdateUserPermissions(int userId, List<string> permissionsName)
        {
            var users = await _userRepository.GetWithUserPermissions(userId);
            if (users == null)
                return null;
            users.UserPermissions.Clear();

            var permissions = await _userRepository.GetPermissionsByPermissionsName(permissionsName);

            foreach (var permission in permissions)
            {
                users.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = permission.PermissionId,
                });
            }
            await _userRepository.SaveChangesAsync();
            return users.UserPermissions.Select(u => u.Permission.PermissionName).ToList();
        }

        public async Task ResetPassword(int userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _userRepository.GetWithUserRolesAndUserPermissions(userId);
            if (user == null) return false;
            user.UserRoles.Clear();
            user.UserPermissions.Clear();
            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<ViewUserDto>> GetPagedUsers(QueryDto query)
        {
            var pagedUsers = await _userRepository.GetPagedUsersAsync(query);

            return new PagedResult<ViewUserDto>
            {
                Items = _mapper.Map<IEnumerable<ViewUserDto>>(pagedUsers.Items),
                TotalCount = pagedUsers.TotalCount
            };
        }

        public async Task<User?> VerifyEmailByToken(string tokenEmail)
        {
            var user = await _userRepository.GetByTokenEmailAsync(tokenEmail);

            if (user != null && user.TokenEmailExpires > DateTime.UtcNow)
            {
                user.IsEmailConfirmed = true;
                user.TokenEmailExpires = null;
                user.TokenEmail = "";
                await _userRepository.SaveChangesAsync();
                return user;
            }

            return null;
        }

        public async Task<ApiResponse<ViewUserDto>> ChangePasswordByIdAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null) return null;
            // Change Pwd
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.NowPassword);
            if (result == PasswordVerificationResult.Failed)
                return ApiResponse<ViewUserDto>.Fail("Mật khẩu hiện tại không đúng");
            if (dto.NewPassword != dto.ConfirmNewPassword)
                return ApiResponse<ViewUserDto>.Fail("Hãy nhập lại mật khẩu mới");
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            user.PasswordUpdateTime = DateTime.UtcNow;

            var viewUserDto = _mapper.Map<ViewUserDto>(user);

            await _userRepository.UpdateAsync(user);
            return ApiResponse<ViewUserDto>.Ok(viewUserDto, "Đổi mật khẩu thành công");
        }

        public async Task IncreaseFailAccessCount(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.FailedAccessCount += 1;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task ResetFailAccessCount(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.FailedAccessCount = 0;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task SetLockEndTimeAsync(int userId, int lockoutDuration)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                if (lockoutDuration > 0)
                    user.LockoutEndTime = DateTime.UtcNow.AddSeconds(lockoutDuration);
                else user.LockoutEndTime = null;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<ApiResponse<DateTime?>> LockUserByIdAsync(int userId, DateTime dateEndTime)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                if (dateEndTime > DateTime.UtcNow)
                {
                    user.LockoutEndTime = dateEndTime;
                    await _userRepository.UpdateAsync(user);
                    return ApiResponse<DateTime?>.Ok(dateEndTime, $"Đã khoá người dùng {user.UserName}");
                } else return ApiResponse<DateTime?>.Fail("Thời gian khoá không hợp lệ");
            }
            // NotFound
            return ApiResponse<DateTime?>.Fail("Người dùng không tồn tại");
        }

        public async Task<ApiResponse> UnLockUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                if (user.LockoutEndTime != null)
                {
                    user.LockoutEndTime = null;
                    await _userRepository.UpdateAsync(user);
                    return ApiResponse.Ok($"Đã gỡ khoá cho người dùng {user.Name}");
                }
                return ApiResponse.Ok("Người dùng đã được gỡ khoá rồi!");
            }
            return ApiResponse.Fail("Người dùng không tồn tại");
        }
    }
}
