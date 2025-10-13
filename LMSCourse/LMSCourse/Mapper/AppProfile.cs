using AutoMapper;
using LMSCourse.Dtos;
using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.Enrollment;
using LMSCourse.DTOs.PaymentDto;
using LMSCourse.DTOs.Question;
using LMSCourse.DTOs.Quiz;
using LMSCourse.DTOs.Role;
using LMSCourse.DTOs.Setting;
using LMSCourse.DTOs.User;
using LMSCourse.DTOs.UserQuiz;
using LMSCourse.Models;

namespace LMSCourse.Mapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


            CreateMap<User, RegisterDto>();


            CreateMap<Role, ViewRoleDto>()
                .ForMember(dest => dest.CountUserRoles, opt => opt.Ignore());

            CreateMap<RoleDto, Role>();

            CreateMap<User, ViewUserDto>()
                .ForMember(dest => dest.Roles,
                       opt => opt.MapFrom(src =>
                           string.Join(", ", src.UserRoles.Select(ur => ur.Role.RoleName))));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName).ToList()));

            CreateMap<EditUserDto, User>();

            CreateMap<PersonalInfoDto, User>();


            CreateMap<IdentitySetting, IdentitySettingDto>();
            CreateMap<PasswordSetting, PasswordSettingDto>();
            CreateMap<LockoutSetting, LockoutSettingDto>();
            CreateMap<SignInSetting, SignInSettingDto>();
            CreateMap<UserSetting, UserSettingDto>();

            CreateMap<IdentitySettingDto, IdentitySetting>();
            CreateMap<PasswordSettingDto, PasswordSetting>();
            CreateMap<LockoutSettingDto, LockoutSetting>();
            CreateMap<SignInSettingDto, SignInSetting>();
            CreateMap<UserSettingDto, UserSetting>();

            CreateMap<AuditLog, AuditLogDto>();

            CreateMap<CourseCreateUpdateDto, Course>();
            CreateMap<FaqGroupCreateUpdateDto, FaqGroup>();
            CreateMap<FaqItemCreateUpdateDto, FaqItem>();
            CreateMap<CourseTopicCreateUpdateDto, CourseTopic>();
            CreateMap<LessonCreateUpdateDto, Lesson>();

            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(c => c.Teacher.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(c => c.Category.Name))
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(c => c.Level.Name))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(c => c.Language.Name))
                .ForMember(dest => dest.FaqGroups, opt => opt.MapFrom(src => src.FaqGroups))
                .ForMember(dest => dest.CourseTopics, opt => opt.MapFrom(src => src.CourseTopics));

            // FaqGroup -> FaqGroupDto
            CreateMap<FaqGroup, FaqGroupDto>();

            // FaqItem -> FaqItemDto
            CreateMap<FaqItem, FaqItemDto>();

            // CourseTopic -> CourseTopicDto
            CreateMap<CourseTopic, CourseTopicDto>();
            CreateMap<Lesson, LessonDto>();

            CreateMap<PaymentDto, Payment>();
            CreateMap<Payment, PaymentDto>();

            CreateMap<Course, CourseEnrolledDto>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(c => c.Teacher.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(c => c.Category.Name))
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(c => c.Level.Name))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(c => c.Language.Name))
                .ForMember(dest => dest.FaqGroups, opt => opt.MapFrom(src => src.FaqGroups))
                .ForMember(dest => dest.CourseTopics, opt => opt.MapFrom(src => src.CourseTopics));

            CreateMap<CourseTopic, CourseTopicDetailDto>();
            CreateMap<Lesson, LessonDetailDto>();

            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(e => e.Course));

            // Quiz
            CreateMap<QuizDto, Quiz>();
            CreateMap<Quiz, QuizViewDto>();

            CreateMap<QuestionDto, Question>();
            CreateMap<AnswerDto, Answer>();

            CreateMap<Question, QuestionViewDto>();
            CreateMap<Answer, AnswerViewDto>();

            CreateMap<UserQuizDto, UserQuiz>();
            CreateMap<UserAnswerDto, UserAnswer>();

            CreateMap<UserQuiz, UserQuizViewDto>();
            CreateMap<UserAnswer, UserAnswerViewDto>();

        }
    } 
}
