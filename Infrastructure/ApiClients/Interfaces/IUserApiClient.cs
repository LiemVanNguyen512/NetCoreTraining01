using Infrastructure.Domains;
using Shared.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients.Interfaces
{
    public interface IUserApiClient
    {
        Task<UserDto> GetMemberById(int id);
        Task<IEnumerable<UserDto>> GetMembers();
        IEnumerable<UserDto> GetMembersSync();
        Task<UserDto> UpdateMember(int id, UpdateUserDto userDto);
    }
}
