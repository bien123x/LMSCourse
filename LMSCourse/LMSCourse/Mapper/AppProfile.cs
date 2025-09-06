using AutoMapper;
using LMSCourse.DTOs.User;
using LMSCourse.Models;

namespace LMSCourse.Mapper
{
    public class AppProfile : Profile
    {
        public AppProfile() {
            CreateMap<User,  UserDto>();
        } 
    }
}
