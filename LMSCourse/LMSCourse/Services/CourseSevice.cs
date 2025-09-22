using AutoMapper;
using LMSCourse.DTOs.Course;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;

namespace LMSCourse.Services
{
    public class CourseSevice : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        public CourseSevice(ICourseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CourseCreateUpdateDto> CreateAsync(CourseCreateUpdateDto courseCreateUpdateDto)
        {
            var course = _mapper.Map<Course>(courseCreateUpdateDto);
            await _repo.AddAsync(course);
            return courseCreateUpdateDto;
        }

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            var courses = await _repo.GetAllWithDataDto();
            var coursesDto = _mapper.Map<IEnumerable<CourseDto>>(courses);
            return coursesDto;
        }
    }
}
