using AutoMapper;
using AutoMapper.Execution;
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
            //check existing member before
            var user = await CheckValidUser(enrollmentDto.UserId);

            //check existing course before
            var course = await CheckValidCourse(enrollmentDto.CourseId);

            //check user "balance" is enough
            CheckValidBalance(user, course.Price);            
            try
            {
                //Begin transaction
                await _repository.BeginTransactionAsync();
                //update Member's Balance -= Courses.Price
                await UpdateMemberBalance(user, course.Price, true);

                //Create new enrollment
                var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
                enrollment.EnrolledDate = DateTime.Now;
                await _repository.CreateEnrollmentAsync(enrollment);

                //Return enrollment already created
                var result = _mapper.Map<EnrollmentDto>(enrollment);
                result.Course = await _courseService.GetCourseAsync(result.CourseId);
                result.Member = await _userApiClient.GetMemberById(result.UserId);
                //End transaction
                await _repository.EndTransactionAsync();
                return result;
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw new Exception($"Can't create enrollment");
            }
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

        
        
        public async Task CancelEnrollmentAsync(int memberId, int courseId)
        {
            //check existing member before
            var user = await CheckValidUser(memberId);
            //check existing course before
            var course = await CheckValidCourse(courseId);
            try
            {
                //Begin transaction
                await _repository.BeginTransactionAsync();
                //Get enrollment
                var enrollmentDto = await GetEnrollmentToCancel(memberId, courseId);
                //update Member's Balance += Courses.Price
                await UpdateMemberBalance(user, course.Price, false);
                //Remove this enrollment
                var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
                await _repository.CancelEnrollmentAsync(enrollment);
                //End transaction
                await _repository.EndTransactionAsync();
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw new Exception($"Can't cancel enrollment with member Id {memberId} and course Id {courseId}");
            }        
        }

        private async Task UpdateMemberBalance(UserDto user, float coursePrice, bool isRegistered)
        {
            if (isRegistered)
            {
                //Member registerd to course
                user.BalanceAccount -= coursePrice;
            }
            else
            {
                //Member cancelled register to course
                user.BalanceAccount += coursePrice;
            }
            var updateUser = _mapper.Map<UpdateUserDto>(user);
            await _userApiClient.UpdateMember(user.Id, updateUser);
        }
        private async Task<UserDto> CheckValidUser(int id)
        {
            var user = await _userApiClient.GetMemberById(id);
            if (user.Id == 0 || user.Email == null)
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
        private void CheckValidBalance(UserDto user, float coursePrice)
        {
            if(user.BalanceAccount < coursePrice)
            {
                throw new Exception($"The balance of user {user.FirstName} is not enough for this course");
            }
        }
        private async Task<EnrollmentDto> GetEnrollmentToCancel(int memberId, int courseId)
        {
            var enrollment = await _repository.GetEnrollmentByMemberAsync(memberId, courseId);
            if (enrollment == null)
            {
                throw new Exception($"Can't find any enrollment with member Id {memberId} and course Id {courseId}");
            }
            var result = _mapper.Map<EnrollmentDto>(enrollment);
            return result;
        }


    }
}
