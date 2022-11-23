using Course_service.Entities;
using Course_service.Persistence;
using Course_service.Repositories.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;

namespace Course_service.Repositories
{
    public class CourseRepository : RepositoryBase<Course, int, CourseContext>, ICourseRepository
    {
        public CourseRepository(CourseContext dbContext, IUnitOfWork<CourseContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task CreateCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<Course> GetCourseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
