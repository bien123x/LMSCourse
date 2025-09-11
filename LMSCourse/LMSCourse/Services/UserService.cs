using AutoMapper;
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
        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
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

        public async Task<User?> RegisterUserAsync(RegisterDto dto)
        {
            var user = _mapper.Map<User>(dto);

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.PasswordHash);

            if (!await _userRepository.CheckExistUserNameOrEmail(dto.UserName) || !await _userRepository.CheckExistUserNameOrEmail(dto.Email))
            {
                await _userRepository.AddAsync(user);
                return user;
            }
            return null;
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

        public async Task<ViewUserDto> AddUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            if (await _userRepository.IsExistUserNameOrEmail(user.UserName, user.Email))
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
            await _userRepository.AddAsync(user);
            return _mapper.Map<ViewUserDto>(user);
        }

        public async Task<ViewUserDto> EditUserDto(int userId, EditUserDto editUserDto)
        {
            var user = await _userRepository.GetUserWithRolesAsync(userId);

            if (user == null)
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
            user.ModificationTime = DateTime.Now;
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
    }
}
