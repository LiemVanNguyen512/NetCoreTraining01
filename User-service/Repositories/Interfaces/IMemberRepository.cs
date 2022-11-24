using User_service.Entities;
using User_service.Persistence;
using Infrastructure.Repositories.Interfaces;

namespace User_service.Repositories.Interfaces
{
    public interface IMemberRepository : IRepositoryBase<User, int, MemberContext>
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
