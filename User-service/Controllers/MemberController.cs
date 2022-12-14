using AutoMapper;
using User_service.Entities;
using User_service.Persistence;
using User_service.Services.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Shared.DTOs.UserDTOs;

namespace User_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomResponseFilter))]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _userService;
        private readonly ILogger<MemberController> _logger;

        public MemberController(IMemberService userService, ILogger<MemberController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([Required] int id)
        {
            var result = await _userService.GetUserAsync(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetListUsers()
        {
            var result = await _userService.GetUsersAsync();           
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto);           
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([Required] int id, [FromBody] UpdateUserDto userDto)
        {
            var result = await _userService.UpdateUserAsync(id, userDto);           
            return Ok(result);
        }
    }
}
