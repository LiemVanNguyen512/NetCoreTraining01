using Shared.DTOs.CourseDTOs;
using Shared.DTOs.EnrollmentDTOs;

namespace Course_service.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetEnrollmentsAsync();
        Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto enrollmentDto);
        Task CancelEnrollmentAsync(int memberId, int courseId);
    }
}
