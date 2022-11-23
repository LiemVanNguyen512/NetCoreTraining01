using Course_service.Entities;
using Course_service.Persistence;
using Infrastructure.Repositories.Interfaces;

namespace Course_service.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IRepositoryBase<Enrollment, int, CourseContext>
    {
        Task<IEnumerable<Course>> GetEnrollmentsAsync();
        Task CreateEnrollmentAsync(Course course);
        Task UpdateEnrollmentAsync(Course course);
    }
}
