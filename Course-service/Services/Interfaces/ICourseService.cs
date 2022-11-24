
using Shared.DTOs.CourseDTOs;

namespace Course_service.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetCoursesAsync();
        Task<CourseDto> GetCourseAsync(int id);
        Task<CourseDto> CreateCourseAsync(CreateCourseDto courseDto);
        Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto courseDto);
    }
}
