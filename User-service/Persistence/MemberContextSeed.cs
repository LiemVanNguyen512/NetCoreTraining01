using User_service.Entities;
using ILogger = Serilog.ILogger;

namespace User_service.Persistence
{
    public class MemberContextSeed
    {
        public static async Task SeedMemberAsync(MemberContext contactContext, ILogger logger)
        {
            if (!contactContext.User.Any())
            {
                contactContext.AddRange(getMembers());
                await contactContext.SaveChangesAsync();
                logger.Information("Seeded data for Member DB associated with context {DbContextName}",
                    nameof(MemberContext));
            }
        }

        private static IEnumerable<User> getMembers()
        {
            return new List<User>
        {
            new()
            {
                FirstName = "Liem",
                LastName = "Nguyen",
                Email = "liem.vannguyen@infodation.vn",
                Phone = "0967778899",
                UserName = "liemnguyen",
                BalanceAccount = 1000000,
            },
            new()
            {
                FirstName = "Hung",
                LastName = "Nguyen",
                Email = "hung.vannguyen@infodation.vn",
                Phone = "093770065",
                UserName = "hungnguyen",
                BalanceAccount= 0,
            }
        };
        }
    }
}
