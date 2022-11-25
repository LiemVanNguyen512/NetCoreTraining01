using Course_service.Entities;
using Course_service.Persistence;
using Infrastructure.Repositories.Interfaces;

namespace Course_service.Repositories.Interfaces
{
    public interface IEnrollmentRepository : IRepositoryBase<Enrollment, int, CourseContext>
    {
        Task<IEnumerable<Enrollment>> GetEnrollmentsAsync();
        Task<Enrollment> GetEnrollmentAsync(int id);
        Task<Enrollment> GetEnrollmentByMemberAsync(int memberId, int  courseId);
        Task CreateEnrollmentAsync(Enrollment enrollment);
        Task CancelEnrollmentAsync(Enrollment enrollment);
    }
}
