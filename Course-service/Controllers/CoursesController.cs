using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Course_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetListCourses()
        {
            throw new NotImplementedException();
        }
        [HttpGet("{id}")]
        public Task<IActionResult> GetCourse([Required] int id)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public Task<IActionResult> CreateCourse()
        {
            throw new NotImplementedException();
        }
        [HttpPut("{id}")]
        public Task<IActionResult> UpdateCourse([Required] int id)
        {
            throw new NotImplementedException();
        }
    }
}
