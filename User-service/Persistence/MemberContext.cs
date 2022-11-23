using User_service.Entities;
using Microsoft.EntityFrameworkCore;

namespace User_service.Persistence
{
    public class MemberContext : DbContext
    {
        public MemberContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
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
