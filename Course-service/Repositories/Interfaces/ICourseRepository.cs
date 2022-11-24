using Course_service.Entities;
using Course_service.Persistence;
using Infrastructure.Repositories.Interfaces;

namespace Course_service.Repositories.Interfaces
{
    public interface ICourseRepository : IRepositoryBase<Course, int, CourseContext>
    {
        Task<IEnumerable<Course>> GetCoursesAsync();
        Task<Course> GetCourseAsync(int id);
        Task CreateCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
    }
}
