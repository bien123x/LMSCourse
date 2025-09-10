using AutoMapper;
using LMSCourse.DTOs.Role;
using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Mapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Role, ViewRoleDto>()
                .ForMember(dest => dest.CountUserRoles, opt => opt.Ignore());

            CreateMap<RoleDto, Role>();

            CreateMap<User, ViewUserDto>()
                .ForMember(dest => dest.Roles,
                       opt => opt.MapFrom(src =>
                           string.Join(", ", src.UserRoles.Select(ur => ur.Role.RoleName))));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName).ToList()));
        }
    } 
}
