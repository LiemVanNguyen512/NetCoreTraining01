using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Course_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetListEnrollments()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public Task<IActionResult> CreateCourse()
        {
            throw new NotImplementedException();
        }
        [HttpPut("cancel")]
        public Task<IActionResult> UpdateCourse([Required] int id)
        {
            throw new NotImplementedException();
        }
    }
}
