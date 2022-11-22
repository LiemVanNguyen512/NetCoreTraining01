using Contact.API.Entities;
using Contact.API.Persistence;
using Contact.API.Repositories.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Repositories
{
    public class ContactRepository : RepositoryBase<CatalogContact, int, ContactContext>, IContactRepository
    {
        public ContactRepository(ContactContext dbContext, IUnitOfWork<ContactContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task CreateContactAsync(CatalogContact contact) => await CreateAsync(contact);

        public async Task DeleteContactAsync(int id)
        {
            var contact = await GetContactAsync(id);
            if(contact != null)
            {
                await DeleteAsync(contact);
            }
        }

        public async Task<CatalogContact> GetContactAsync(int id) => await GetByIdAsync(id);

        public async Task<CatalogContact> GetContactByEmailAsync(string email)
        {
            return await FindByCondition(x=>x.Email.Equals(email)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CatalogContact>> GetContactsAsync() => await FindAll().ToListAsync();

        public async Task UpdateContactAsync(CatalogContact contact) => await UpdateAsync(contact);
    }
}
