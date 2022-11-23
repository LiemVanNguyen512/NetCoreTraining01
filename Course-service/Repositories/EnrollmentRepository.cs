using Course_service.Entities;
using Course_service.Persistence;
using Course_service.Repositories.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;

namespace Course_service.Repositories
{
    public class EnrollmentRepository : RepositoryBase<Enrollment, int, CourseContext>, IEnrollmentRepository
    {
        public EnrollmentRepository(CourseContext dbContext, IUnitOfWork<CourseContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public Task CreateEnrollmentAsync(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetEnrollmentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateEnrollmentAsync(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
