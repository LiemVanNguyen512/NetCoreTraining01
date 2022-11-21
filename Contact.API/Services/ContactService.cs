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

        public async Task<bool> CreateContactAsync(CatalogContact contact)
        {
            try
            {
                var checkEmail = await CheckEmailExist(contact.Email);
                if(checkEmail != null)
                {
                    return false;
                }
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }          
            return true;
        }

        public async Task<CatalogContact> GetContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            return contact;
        }
        private async Task<CatalogContact> CheckEmailExist(string email, string oldEmail = "")
        {
            var contact = new CatalogContact();
            if (!string.IsNullOrEmpty(oldEmail))
            {
                contact = await _context.Contacts.Where(x => x.Email != oldEmail && x.Email == email).FirstOrDefaultAsync();
            }
            else
            {
                contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Email == email);
            }
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
            try
            {
                var contactUpdate = await GetContactAsync(id);
                if (contactUpdate == null)
                {
                    return false;
                }
                var checkEmail = await CheckEmailExist(contact.Email, contactUpdate.Email);
                if (checkEmail != null)
                {
                    return false;
                }
                contactUpdate.FirstName = contact.FirstName;
                contactUpdate.LastName = contact.LastName;
                contactUpdate.Email = contact.Email;
                contactUpdate.Phone = contact.Phone;
                _context.Contacts.Update(contactUpdate);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }         
            return true;
        }
    }
}
