using AutoMapper.Execution;
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
        public async Task<Enrollment> GetEnrollmentAsync(int id) => await GetByIdAsync(id);

        public async Task CancelEnrollmentAsync(Enrollment enrollment) => await DeleteAsync(enrollment);

        public async Task<Enrollment> GetEnrollmentByMemberAsync(int memberId, int courseId)
        {
            return await FindByCondition(x => x.UserId.Equals(memberId) && x.CourseId.Equals(courseId)).SingleOrDefaultAsync();
        }
    }
}
