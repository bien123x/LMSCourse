using AutoMapper;
using LMSCourse.DTOs;
using LMSCourse.DTOs.Course;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services.Interfaces;
using System.Security.Claims;

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

        public async Task<IEnumerable<CourseDto>> GetAllAsync(int userId)
        {
            var courses = await _repo.GetAllWithDataDto(userId);
            var coursesDto = _mapper.Map<IEnumerable<CourseDto>>(courses);
            return coursesDto;
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByListId(List<int> courseIds, int userId)
        {
            var coursesDto = new List<CourseDto>();
            foreach (var courseId in courseIds)
            {
                var apiResponse = await GetCourseByIdAsync(courseId);
                coursesDto.Add(apiResponse.Data!);
            }
            return coursesDto;
        }

        public async Task<PagedResult<CourseDto>> GetAllWithFilter(QueryCourseDto dto, int userId)
        {
            var pagedCourses = await _repo.GetAllWithFilters(dto, userId);

            return new PagedResult<CourseDto>
            {
                Items = _mapper.Map<IEnumerable<CourseDto>>(pagedCourses.Items),
                TotalCount = pagedCourses.TotalCount
            };
        }

        public async Task<ApiResponse<CourseDto>> GetCourseByIdAsync(int courseId)
        {
            var course = await _repo.GetWithDataDtoByIdAsync(courseId);

            if (course == null) return ApiResponse<CourseDto>.Fail("Không tìm thấy khoá học này");
            else
            {
                return ApiResponse<CourseDto>.Ok(_mapper.Map<CourseDto>(course));
            }
        }

        public async Task<CourseFiltersDto> GetCourseFilterAsync(int userId)
        {
            
            var courseFilters = new CourseFiltersDto();
            var courses = await _repo.GetAllWithDataDto(userId);
            
             courseFilters.Teachers = courses
                .GroupBy(c => new {c.TeacherId, c.Teacher!.Name})
                .Select(g => new TeacherDto
                {
                    TeacherId = g.Key.TeacherId,
                    Name = g.Key.Name,
                    Count = g.Count()
                }).ToList();

            // Categories
            courseFilters.Categories = courses
                .GroupBy(c => new { c.CategoryId, c.Category!.Name })
                .Select(g => new CategoryDto
                {
                    CategoryId = g.Key.CategoryId,
                    Name = g.Key.Name,
                    Count = g.Count()
                })
                .ToList();

            // Levels
            courseFilters.Levels = courses
                .GroupBy(c => new { c.LevelId, c.Level!.Name })
                .Select(g => new LevelDto
                {
                    LevelId = g.Key.LevelId,
                    Name = g.Key.Name,
                    Count = g.Count()
                })
                .ToList();

            // Language
            courseFilters.Languages = courses
                .GroupBy(c => new { c.LanguageId, c.Language!.Name })
                .Select(g => new LanguageDto
                {
                    LanguageId = g.Key.LanguageId,
                    Name = g.Key.Name,
                    Count = g.Count()
                })
                .ToList();
            return courseFilters;
        }

        public async Task<PagedResult<CourseDto>> GetCoursesEnrolled(int userId, QueryCourseEnrolledDto dto)
        {
            var courses = await _repo.GetAllWithEnrolledDataDto(userId, dto);

            return new PagedResult<CourseDto>
            {
                Items = _mapper.Map<IEnumerable<CourseDto>>(courses.Items),
                TotalCount = courses.TotalCount
            };
        }

        public async Task<int?> GetRemainingCapacity(int courseId)
        {
            var remainingCapacity = await _repo.GetRemainingCapacity(courseId);

            return remainingCapacity;
        }
    }
}
