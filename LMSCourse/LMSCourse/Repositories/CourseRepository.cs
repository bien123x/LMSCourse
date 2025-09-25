using LMSCourse.Data;
using LMSCourse.DTOs.Page_Sort_Filter;
using LMSCourse.Models;
using LMSCourse.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMSCourse.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context) { 
        }

        public async Task<IEnumerable<Course>> GetAllWithDataDto()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Category)
                .Include(c => c.Level)
                .Include(c => c.Language)
                .Include(c => c.FaqGroups)
                    .ThenInclude(fg => fg.FaqItems)
                .Include(c => c.CourseTopics)
                .ToListAsync();
        }

        public async Task<PagedResult<Course>> GetAllWithFilters(QueryCourseDto dto)
        {
            var courses = _context.Courses.Where(c => c.IsPublic == true)
                .Include(c => c.Teacher)
                .Include(c => c.Category)
                .Include(c => c.Level)
                .Include(c => c.Language)
                .Include(c => c.FaqGroups)
                    .ThenInclude(fg => fg.FaqItems)
                .Include(c => c.CourseTopics)
                .AsQueryable();

            // Lọc theo TeacherIds
            if (dto.CourseFilterRequest.TeacherIds != null && dto.CourseFilterRequest.TeacherIds.Any())
            {
                courses = courses.Where(c => dto.CourseFilterRequest.TeacherIds.Contains(c.TeacherId));
            }

            // Lọc theo CategoryIds
            if (dto.CourseFilterRequest.CategoryIds != null && dto.CourseFilterRequest.CategoryIds.Any())
            {
                courses = courses.Where(c => dto.CourseFilterRequest.CategoryIds.Contains(c.CategoryId));
            }

            // Lọc theo LevelIds
            if (dto.CourseFilterRequest.LevelIds != null && dto.CourseFilterRequest.LevelIds.Any())
            {
                courses = courses.Where(c => dto.CourseFilterRequest.LevelIds.Contains(c.LevelId));
            }

            // Lọc theo LanguageIds
            if (dto.CourseFilterRequest.LanguageIds != null && dto.CourseFilterRequest.LanguageIds.Any())
            {
                courses = courses.Where(c => dto.CourseFilterRequest.LanguageIds.Contains(c.LanguageId));
            }

            var totalCount = await courses.CountAsync();

            // Sort thủ công
            switch (dto.SortField?.ToLower())
            {
                case "price":
                    courses = dto.SortOrder == -1
                        ? courses.OrderByDescending(c => c.Price)
                        : courses.OrderBy(c => c.Price);
                    break;

                case "title":
                    courses = dto.SortOrder == -1
                        ? courses.OrderByDescending(c => c.Title)
                        : courses.OrderBy(c => c.Title);
                    break;

                case "createdat":
                    courses = dto.SortOrder == -1
                        ? courses.OrderByDescending(c => c.CreatedAt)
                        : courses.OrderBy(c => c.CreatedAt);
                    break;

                default:
                    courses = courses.OrderBy(c => c.Title); // sort mặc định
                    break;
            }

            var items = await courses
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();



            return new PagedResult<Course>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<Course?> GetWithDataDtoByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Category)
                .Include(c => c.Level)
                .Include(c => c.Language)
                .Include(c => c.FaqGroups)
                    .ThenInclude(fg => fg.FaqItems)
                .Include(c => c.CourseTopics)
                    .ThenInclude(ct => ct.Lessons)
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }
    }
}
