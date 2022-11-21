using Contact.API.Entities;
using Contact.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactContext _context;

        public ContactService(ContactContext context)
        {
            _context = context;
        }

        public async Task<int> CreateContactAsync(CatalogContact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<CatalogContact> GetContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            return contact;
        }

        public async Task<IEnumerable<CatalogContact>> GetContactsAsync(bool trackChanges = false)
        {
            var contacts = (!trackChanges) ? await _context.Contacts.AsNoTracking().ToListAsync()
                                            : await _context.Contacts.ToListAsync();
            return contacts;
        }

        public async Task<bool> UpdateContactAsync(int id, CatalogContact contact)
        {
            var contactUpdate = await GetContactAsync(id);
            if(contactUpdate == null)
            {
                return false;
            }
            contactUpdate.FirstName = contact.FirstName;
            contactUpdate.LastName = contact.LastName;
            contactUpdate.Email = contact.Email;
            contactUpdate.Phone = contact.Phone;
            _context.Contacts.Update(contactUpdate);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
