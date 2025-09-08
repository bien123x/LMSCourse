using AutoMapper;
using LMSCourse.DTOs.Role;
using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Mapper
{
    public class AppProfile : Profile
    {
        public AppProfile() {
            CreateMap<User,  UserDto>();

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Role, ViewRoleDto>()
                .ForMember(dest => dest.CountUserRoles, opt => opt.Ignore());

            CreateMap<RoleDto, Role>();
        } 
    }
}
