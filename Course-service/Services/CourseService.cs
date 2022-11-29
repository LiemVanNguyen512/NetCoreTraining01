using AutoMapper;
using Course_service.Entities;
using Course_service.Persistence;
using Course_service.Services.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.CourseDTOs;
using Shared.DTOs.UserDTOs;

namespace Course_service.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepositoryBase<Course, int, CourseContext> _courseRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(IRepositoryBase<Course, int, CourseContext> courseRepo, IMapper mapper, ILogger<CourseService> logger)
        {
            _courseRepo = courseRepo ?? throw new ArgumentNullException(nameof(courseRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto courseDto)
        {
            var course = _mapper.Map<Course>(courseDto);
            await _courseRepo.AddAsync(course);
            _logger.LogInformation($"Create Course {course.Code} successfully");
            var result = _mapper.Map<CourseDto>(course);
            return result;
        }

        public async Task<CourseDto> GetCourseAsync(int id)
        {
            var course = await _courseRepo.FindAsync(id);
            _logger.LogInformation($"Get Course by Id: {id} successfully");
            var result = _mapper.Map<CourseDto>(course);
            return result;
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
        {
            var course = await _courseRepo.FindAll().ToListAsync();
            _logger.LogInformation($"Get {course.Count()} Courses successfully");
            var result = _mapper.Map<IEnumerable<CourseDto>>(course);
            return result;
        }

        public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto courseDto)
        {
            try
            {
                var course = await _courseRepo.FindAsync(id);          
                var updateCourse = _mapper.Map(courseDto, course);
                await _courseRepo.UpdateAsync(updateCourse);
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
