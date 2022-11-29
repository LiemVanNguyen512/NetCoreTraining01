using AutoMapper;
using AutoMapper.Execution;
using Course_service.Entities;
using Course_service.Persistence;
using Course_service.Services.Interfaces;
using Infrastructure.ApiClients.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.CourseDTOs;
using Shared.DTOs.EnrollmentDTOs;
using Shared.DTOs.UserDTOs;

namespace Course_service.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IRepositoryBase<Enrollment, int, CourseContext> _enrollmentRepo;
        private readonly ICourseService _courseService;
        private readonly IUserApiClient _userApiClient;
        private readonly ILogger<EnrollmentService> _logger;
        private readonly IMapper _mapper;

        public EnrollmentService(
            IRepositoryBase<Enrollment, int, CourseContext> enrollmentRepo, 
            ICourseService courseService, 
            IUserApiClient userApiClient, 
            ILogger<EnrollmentService> logger, IMapper mapper)
        {
            _enrollmentRepo = enrollmentRepo ?? throw new ArgumentNullException(nameof(enrollmentRepo));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _userApiClient = userApiClient ?? throw new ArgumentNullException(nameof(userApiClient));
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
                await _enrollmentRepo.BeginTransactionAsync();
                //update Member's Balance -= Courses.Price
                await UpdateMemberBalance(user, course.Price, true);

                //Create new enrollment
                var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
                enrollment.EnrolledDate = DateTime.Now;
                await _enrollmentRepo.AddAsync(enrollment);

                //Return enrollment already created
                var result = _mapper.Map<EnrollmentDto>(enrollment);
                result.Course = await _courseService.GetCourseAsync(result.CourseId);
                result.Member = await _userApiClient.GetMemberById(result.UserId);
                //End transaction
                await _enrollmentRepo.EndTransactionAsync();
                return result;
            }
            catch
            {
                await _enrollmentRepo.RollbackTransactionAsync();
                throw new Exception($"Can't create enrollment");
            }
        }
        public EnrollmentDto CreateEnrollmentSync(CreateEnrollmentDto enrollmentDto)
        {
            //check existing member before
            var user = CheckValidUser(enrollmentDto.UserId).Result;

            //check existing course before
            var course = CheckValidCourse(enrollmentDto.CourseId).Result;

            //check user "balance" is enough
            CheckValidBalance(user, course.Price);
            try
            {
                //Begin transaction
                _enrollmentRepo.BeginTransactionAsync().Wait();
                //update Member's Balance -= Courses.Price
                UpdateMemberBalance(user, course.Price, true).Wait();

                //Create new enrollment
                var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
                enrollment.EnrolledDate = DateTime.Now;
                _enrollmentRepo.AddAsync(enrollment).Wait();

                //Return enrollment already created
                var result = _mapper.Map<EnrollmentDto>(enrollment);
                result.Course = _courseService.GetCourseAsync(result.CourseId).Result;
                result.Member = _userApiClient.GetMemberById(result.UserId).Result;
                //End transaction
                _enrollmentRepo.EndTransactionAsync().Wait();
                return result;
            }
            catch
            {
                _enrollmentRepo.RollbackTransactionAsync().Wait();
                throw new Exception($"Can't create enrollment");
            }
        }
        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsAsync()
        {
            var enrollments = await _enrollmentRepo.FindAll(false, x => x.Course).ToListAsync();
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
        public IEnumerable<EnrollmentDto> GetEnrollmentsSync()
        {
            var enrollments = _enrollmentRepo.FindAll(false,x=>x.Course).ToListAsync().Result;
            var enrollmentDtos = _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
            var members = _userApiClient.GetMembersSync();
            enrollmentDtos = enrollmentDtos.Select(x => new EnrollmentDto()
            {
                Id = x.Id,
                EnrolledDate = x.EnrolledDate,
                CourseId = x.CourseId,
                Course = x.Course,
                UserId = x.UserId,
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
                await _enrollmentRepo.BeginTransactionAsync();
                //Get enrollment
                var enrollmentDto = await GetEnrollmentToCancel(memberId, courseId);
                //update Member's Balance += Courses.Price
                await UpdateMemberBalance(user, course.Price, false);
                //Remove this enrollment
                var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
                await _enrollmentRepo.RemoveAsync(enrollment);
                //End transaction
                await _enrollmentRepo.EndTransactionAsync();
            }
            catch
            {
                await _enrollmentRepo.RollbackTransactionAsync();
                throw new Exception($"Can't cancel enrollment with member Id {memberId} and course Id {courseId}");
            }        
        }
        public void CancelEnrollmentSync(int memberId, int courseId)
        {
            //check existing member before
            var user = CheckValidUser(memberId).Result;
            //check existing course before
            var course = CheckValidCourse(courseId).Result;
            try
            {
                //Begin transaction
                _enrollmentRepo.BeginTransactionAsync().Wait();
                //Get enrollment
                var enrollmentDto = GetEnrollmentToCancel(memberId, courseId).Result;
                //update Member's Balance += Courses.Price
                UpdateMemberBalance(user, course.Price, false).Wait();
                //Remove this enrollment
                var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
                _enrollmentRepo.RemoveAsync(enrollment).Wait();
                //End transaction
                _enrollmentRepo.EndTransactionAsync().Wait();
            }
            catch
            {
                _enrollmentRepo.RollbackTransactionAsync().Wait();
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
            var enrollment = await _enrollmentRepo.FindByCondition(x => x.UserId.Equals(memberId) && x.CourseId.Equals(courseId)).SingleOrDefaultAsync();
            if (enrollment == null)
            {
                throw new Exception($"Can't find any enrollment with member Id {memberId} and course Id {courseId}");
            }
            var result = _mapper.Map<EnrollmentDto>(enrollment);
            return result;
        }       
    }
}
