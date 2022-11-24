using Course_service.Entities;
using Course_service.Persistence;
using Course_service.Repositories.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Course_service.Repositories
{
    public class CourseRepository : RepositoryBase<Course, int, CourseContext>, ICourseRepository
    {
        public CourseRepository(CourseContext dbContext, IUnitOfWork<CourseContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task CreateCourseAsync(Course course) => await CreateAsync(course);

        public async Task<Course> GetCourseAsync(int id) => await GetByIdAsync(id);

        public async Task<IEnumerable<Course>> GetCoursesAsync() => await FindAll().ToListAsync();

        public async Task UpdateCourseAsync(Course course) => await UpdateAsync(course);
    }
}
