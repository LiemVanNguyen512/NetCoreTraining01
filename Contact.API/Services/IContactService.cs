using Contact.API.Entities;

namespace Contact.API.Services
{
    public interface IContactService
    {
        Task<IEnumerable<CatalogContact>> GetContactsAsync(bool trackChanges = false);
        Task<CatalogContact> GetContactAsync(int id);
        Task<bool> CreateContactAsync(CatalogContact product);
        Task<bool> UpdateContactAsync(int id, CatalogContact product);
    }
}
