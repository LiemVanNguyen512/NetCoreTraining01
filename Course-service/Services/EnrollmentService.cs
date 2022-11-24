using AutoMapper;
using Course_service.Entities;
using Course_service.Repositories.Interfaces;
using Course_service.Services.Interfaces;
using Infrastructure.ApiClients.Interfaces;
using Shared.DTOs.CourseDTOs;
using Shared.DTOs.EnrollmentDTOs;
using Shared.DTOs.UserDTOs;

namespace Course_service.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;
        private readonly ICourseService _courseService;
        private readonly IUserApiClient _userApiClient;
        private readonly ILogger<EnrollmentService> _logger;
        private readonly IMapper _mapper;

        public EnrollmentService(
            IEnrollmentRepository repository, 
            ICourseService courseService, 
            IUserApiClient userApiClient, 
            ILogger<EnrollmentService> logger, 
            IMapper mapper)
        {
            _repository = repository;
            _courseService = courseService;
            _userApiClient = userApiClient;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto enrollmentDto)
        {
            var user = await CheckValidUser(enrollmentDto.UserId);
            var course = await CheckValidCourse(enrollmentDto.CourseId);
            CheckValidBalance(user, course.Price);
            await UpdateMemberBalance(user, course.Price);
            var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
            enrollment.EnrolledDate = DateTime.Now;
            await _repository.CreateEnrollmentAsync(enrollment);
            var result = _mapper.Map<EnrollmentDto>(enrollment);
            return result;
        }        
        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsAsync()
        {
            var enrollments = await _repository.GetEnrollmentsAsync();
            var enrollmentDtos = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
            var members = await _userApiClient.GetMembers();
            enrollmentDtos = enrollmentDtos.Select(x => new EnrollmentDto()
            {
                Id= x.Id,
                EnrolledDate= x.EnrolledDate,
                CourseId= x.CourseId,
                Course = x.Course,
                UserId= x.UserId,
                Member = members.FirstOrDefault(m => m.Id == x.UserId)
            });
            return enrollmentDtos;
        }

        public Task<EnrollmentDto> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto enrollmentDto)
        {
            throw new NotImplementedException();
        }
        private async Task<UserDto> CheckValidUser(int id)
        {
            var user = await _userApiClient.GetMemberById(id);
            if (user == null)
            {
                throw new Exception("Member is invalid");
            }
            return user;
        }
        private async Task<CourseDto> CheckValidCourse(int id)
        {
            var course = await _courseService.GetCourseAsync(id);
            if (course == null)
            {
                throw new Exception("Course is invalid");
            }
            return course;
        }
        private async Task UpdateMemberBalance(UserDto user, float coursePrice)
        {
            user.BalanceAccount -= coursePrice;
            var updateUser = _mapper.Map<UpdateUserDto>(user);
            await _userApiClient.UpdateMember(user.Id, updateUser);
        }
        private void CheckValidBalance(UserDto user, float coursePrice)
        {
            if(user.BalanceAccount < coursePrice)
            {
                throw new Exception($"The balance of user {user.FirstName} is not enough for this course");
            }
        }
    }
}
