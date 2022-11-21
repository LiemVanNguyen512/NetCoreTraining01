using Contact.API.Entities;

namespace Contact.API.Services
{
    public interface IContactService
    {
        Task<IEnumerable<CatalogContact>> GetContactsAsync(bool trackChanges = false);
        Task<CatalogContact> GetContactAsync(int id);
        Task<int> CreateContactAsync(CatalogContact product);
        Task<bool> UpdateContactAsync(int id, CatalogContact product);
    }
}
