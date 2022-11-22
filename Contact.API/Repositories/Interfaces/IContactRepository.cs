using Contact.API.Entities;
using Contact.API.Persistence;
using Infrastructure.Repositories.Interfaces;

namespace Contact.API.Repositories.Interfaces
{
    public interface IContactRepository : IRepositoryBase<CatalogContact, int, ContactContext>
    {
        Task<IEnumerable<CatalogContact>> GetContactsAsync();
        Task<CatalogContact> GetContactAsync(int id);
        Task<CatalogContact> GetContactByEmailAsync(string email);
        Task CreateContactAsync(CatalogContact contact);
        Task UpdateContactAsync(CatalogContact contact);
        Task DeleteContactAsync(int id);
    }
}
