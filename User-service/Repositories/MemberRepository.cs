using User_service.Entities;
using User_service.Persistence;
using User_service.Repositories.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace User_service.Repositories
{
    public class MemberRepository : RepositoryBase<User, int, MemberContext>, IMemberRepository
    {
        public MemberRepository(MemberContext dbContext, IUnitOfWork<MemberContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task CreateUserAsync(User user) => await CreateAsync(user);

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserAsync(id);
            if(user != null)
            {
                await DeleteAsync(user);
            }
        }

        public async Task<User> GetUserAsync(int id) => await GetByIdAsync(id);

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await FindByCondition(x=>x.Email.Equals(email)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync() => await FindAll().ToListAsync();

        public async Task UpdateUserAsync(User user) => await UpdateAsync(user);
    }
}
