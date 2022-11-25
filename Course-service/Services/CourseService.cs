using AutoMapper;
using Course_service.Entities;
using Course_service.Repositories.Interfaces;
using Course_service.Services.Interfaces;
using Shared.DTOs.CourseDTOs;
using Shared.DTOs.UserDTOs;

namespace Course_service.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository repository, IMapper mapper, ILogger<CourseService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto courseDto)
        {
            var course = _mapper.Map<Course>(courseDto);
            await _repository.CreateCourseAsync(course);
            _logger.LogInformation($"Create Course {course.Code} successfully");
            var result = _mapper.Map<CourseDto>(course);
            return result;
        }

        public async Task<CourseDto> GetCourseAsync(int id)
        {
            var course = await _repository.GetCourseAsync(id);
            _logger.LogInformation($"Get Course by Id: {id} successfully");
            var result = _mapper.Map<CourseDto>(course);
            return result;
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
        {
            var course = await _repository.GetCoursesAsync();
            _logger.LogInformation($"Get {course.Count()} Courses successfully");
            var result = _mapper.Map<IEnumerable<CourseDto>>(course);
            return result;
        }

        public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto courseDto)
        {
            try
            {
                var course = await _repository.GetCourseAsync(id);             
                var updateCourse = _mapper.Map(courseDto, course);
                await _repository.UpdateCourseAsync(updateCourse);
                _logger.LogInformation($"Update Course with id {updateCourse.Id} successfully");
                var result = _mapper.Map<CourseDto>(course);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw new Exception($"Can not update User with Id {id}");
            }
        }
    }
}
