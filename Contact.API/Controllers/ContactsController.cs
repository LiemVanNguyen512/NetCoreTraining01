using AutoMapper;
using Contact.API.DTOs;
using Contact.API.Entities;
using Contact.API.Persistence;
using Contact.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactsController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact([Required] int id)
        {
            var contact = await _contactService.GetContactAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            var contactDto = _mapper.Map<ContactDto>(contact);
            return Ok(contactDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetListContact()
        {
            var contacts = await _contactService.GetContactsAsync();
            if (contacts == null)
            {
                return NoContent();
            }
            var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);
            return Ok(contactDtos);
        }
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactDto contactDto)
        {
            var contact = _mapper.Map<CatalogContact>(contactDto);
            var result = await _contactService.CreateContactAsync(contact);
            if(!result)
            {
                return BadRequest();
            }
            return Ok(contactDto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateContact([Required] int id, [FromBody] UpdateContactDto contactDto)
        {
            var contact = _mapper.Map<CatalogContact>(contactDto);
            var result = await _contactService.UpdateContactAsync(id, contact);
            if (!result)
            {
                return BadRequest();
            }
            return Ok(contactDto);
        }
    }
}
