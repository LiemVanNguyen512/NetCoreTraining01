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
        private readonly IUserApiClient _userApiClient;
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IUserApiClient userApiClient, IEnrollmentService enrollmentService)
        {
            _userApiClient = userApiClient;
            _enrollmentService = enrollmentService;
        }

        [HttpGet("test/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userApiClient.GetMemberById(id);
            return Ok(result);
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
        public Task<IActionResult> UpdateEnrollment([Required] int id)
        {
            throw new NotImplementedException();
        }
    }
}
