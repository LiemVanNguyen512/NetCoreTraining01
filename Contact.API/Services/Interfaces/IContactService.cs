using Contact.API.DTOs;
using Contact.API.Entities;

namespace Contact.API.Services.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> GetContactsAsync();
        Task<ContactDto> GetContactAsync(int id);
        Task<ContactDto> CreateContactAsync(CreateContactDto contactDto);
        Task<ContactDto> UpdateContactAsync(int id, UpdateContactDto contactDto);
    }
}
