using Course_service.Services.Interfaces;
using Infrastructure.ApiClients.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.EnrollmentDTOs;
using System.ComponentModel.DataAnnotations;

namespace Course_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomResponseFilter))]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetListEnrollments()
        {
            var result = await _enrollmentService.GetEnrollmentsAsync();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody]CreateEnrollmentDto enrollmentDto)
        {
            var result = await _enrollmentService.CreateEnrollmentAsync(enrollmentDto);
            return Ok(result);
        }
        [HttpPut("cancel")]
        public async Task<IActionResult> CancelEnrollment([Required] int memberId, [Required] int courseId)
        {
            await _enrollmentService.CancelEnrollmentAsync(memberId, courseId);
            return StatusCode(200, $"Cancelled enrollment with Member {memberId} and Course {courseId}");
        }
    }
}
