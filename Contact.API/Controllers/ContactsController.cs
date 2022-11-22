using AutoMapper;
using Contact.API.DTOs;
using Contact.API.Entities;
using Contact.API.Persistence;
using Contact.API.Services.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomResponseFilter))]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact([Required] int id)
        {
            var contact = await _contactService.GetContactAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        [HttpGet]
        public async Task<IActionResult> GetListContacts()
        {
            var contacts = await _contactService.GetContactsAsync();           
            return Ok(contacts);
        }
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactDto contactDto)
        {
            var result = await _contactService.CreateContactAsync(contactDto);           
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateContact([Required] int id, [FromBody] UpdateContactDto contactDto)
        {
            var result = await _contactService.UpdateContactAsync(id, contactDto);           
            return Ok(result);
        }
    }
}
