using Contact.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contact.API.Persistence
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CatalogContact> Contacts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CatalogContact>().HasIndex(x => x.Email).IsUnique();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var item in modified)
            {
                switch (item.State)
                {                   
                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false;                      
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
