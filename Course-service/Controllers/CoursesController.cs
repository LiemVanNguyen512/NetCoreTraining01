using Course_service.Services.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.CourseDTOs;
using System.ComponentModel.DataAnnotations;

namespace Course_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomResponseFilter))]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;

        public CoursesController(ICourseService courseService, IEnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetListCourses()
        {
            var result = await _courseService.GetCoursesAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse([Required] int id)
        {
            var result = await _courseService.GetCourseAsync(id);
            if(result == null) 
            { 
                return NotFound(result); 
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseDto courseDto)
        {
            var result = await _courseService.CreateCourseAsync(courseDto);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse([Required] int id, [FromBody]UpdateCourseDto courseDto)
        {
            var result = await _courseService.UpdateCourseAsync(id, courseDto);
            return Ok(result);
        }
    }
}
