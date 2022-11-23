using Infrastructure.DTOs.UserDTOs;

namespace User_service.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto);
    }
}
