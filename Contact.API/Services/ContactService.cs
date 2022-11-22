using AutoMapper;
using Contact.API.DTOs;
using Contact.API.Entities;
using Contact.API.Persistence;
using Contact.API.Repositories.Interfaces;
using Contact.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IContactRepository contactRepository, IMapper mapper, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ContactDto> CreateContactAsync(CreateContactDto contactDto)
        {
            var contactByEmail = await _contactRepository.GetContactByEmailAsync(contactDto.Email);
            if (contactByEmail != null)
            {
                _logger.LogWarning($"Cannot create contact: Email {contactByEmail.Email} is existed");
                throw new Exception($"Cannot create contact: Email {contactByEmail.Email} is existed");
            }
            var contact = _mapper.Map<CatalogContact>(contactDto);
            await _contactRepository.CreateContactAsync(contact);
            _logger.LogInformation($"Create contact with email {contact.Email} successfully");
            var result = _mapper.Map<ContactDto>(contact);
            return result;
        }

        public async Task<ContactDto> GetContactAsync(int id)
        {
            var contact = await _contactRepository.GetContactAsync(id);
            _logger.LogInformation($"Get contact by Id: {id} successfully");
            var result = _mapper.Map<ContactDto>(contact);
            return result;
        }

        public async Task<IEnumerable<ContactDto>> GetContactsAsync()
        {
            var contacts = await _contactRepository.GetContactsAsync();
            _logger.LogInformation($"Get {contacts.Count()} contacts successfully");
            var result = _mapper.Map<IEnumerable<ContactDto>>(contacts);
            return result;
        }

        public async Task<ContactDto> UpdateContactAsync(int id, UpdateContactDto contactDto)
        {
            try
            {
                var contact = await _contactRepository.GetContactAsync(id);
                if (contactDto.Email != contact.Email)
                {
                    var contactByEmail = await _contactRepository.GetContactByEmailAsync(contactDto.Email);
                    if (contactByEmail != null)
                    {
                        _logger.LogWarning($"Cannot update contact {id}: Email {contactByEmail.Email} is existed");
                        throw new Exception($"Cannot update contact {id}: Email {contactByEmail.Email} is existed");
                    }
                }
                var updateContact = _mapper.Map(contactDto, contact);
                await _contactRepository.UpdateContactAsync(updateContact);
                _logger.LogInformation($"Update contact with id {updateContact.Id} successfully");
                var result = _mapper.Map<ContactDto>(contact);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw new Exception($"Can not update Contact with Id {id}");
            }          
        }
    }
}
