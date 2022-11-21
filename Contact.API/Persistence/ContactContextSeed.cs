using Contact.API.Entities;
using ILogger = Serilog.ILogger;

namespace Contact.API.Persistence
{
    public class ContactContextSeed
    {
        public static async Task SeedProductAsync(ContactContext contactContext, ILogger logger)
        {
            if (!contactContext.Contacts.Any())
            {
                contactContext.AddRange(getCatalogProducts());
                await contactContext.SaveChangesAsync();
                logger.Information("Seeded data for Product DB associated with context {DbContextName}",
                    nameof(ContactContext));
            }
        }

        private static IEnumerable<CatalogContact> getCatalogProducts()
        {
            return new List<CatalogContact>
        {
            new()
            {
                FirstName = "Liem",
                LastName = "Nguyen",
                Email = "liem.vannguyen@infodation.vn",
                Phone = "0967778899"
            },
            new()
            {
                FirstName = "Hung",
                LastName = "Nguyen",
                Email = "hung.vannguyen@infodation.vn",
                Phone = "093770065"
            }
        };
        }
    }
}
