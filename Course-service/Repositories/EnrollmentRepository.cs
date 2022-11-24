using Course_service.Entities;
using Course_service.Persistence;
using Course_service.Repositories.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Course_service.Repositories
{
    public class EnrollmentRepository : RepositoryBase<Enrollment, int, CourseContext>, IEnrollmentRepository
    {
        public EnrollmentRepository(CourseContext dbContext, IUnitOfWork<CourseContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task CreateEnrollmentAsync(Enrollment enrollment) => await CreateAsync(enrollment);

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsAsync() => await FindAll(false, x=>x.Course).ToListAsync();

        public async Task UpdateEnrollmentAsync(Enrollment enrollment) => await UpdateAsync(enrollment);
    }
}
