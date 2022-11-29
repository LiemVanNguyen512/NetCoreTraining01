using AutoMapper;
using User_service.Entities;
using User_service.Persistence;
using User_service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.UserDTOs;
using Infrastructure.Repositories.Interfaces;

namespace User_service.Services
{
    public class MemberService : IMemberService
    {
        private readonly IRepositoryBase<User, int, MemberContext> _memberRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MemberService> _logger;

        public MemberService(
            IRepositoryBase<User, int, MemberContext> memberRepo, 
            IMapper mapper, 
            ILogger<MemberService> logger)
        {
            _memberRepo = memberRepo ?? throw new ArgumentNullException(nameof(memberRepo));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            var userByEmail = await _memberRepo.FindByCondition(x => x.Email.Equals(userDto.Email)).SingleOrDefaultAsync();
            if (userByEmail != null)
            {
                _logger.LogWarning($"Cannot create User: Email {userByEmail.Email} is existed");
                throw new Exception($"Cannot create User: Email {userByEmail.Email} is existed");
            }
            var user = _mapper.Map<User>(userDto);
            await _memberRepo.AddAsync(user);
            _logger.LogInformation($"Create User with email {user.Email} successfully");
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            var user = await _memberRepo.FindAsync(id);
            _logger.LogInformation($"Get User by Id: {id} successfully");
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _memberRepo.FindAll().ToListAsync();
            _logger.LogInformation($"Get {users.Count()} Users successfully");
            var result = _mapper.Map<IEnumerable<UserDto>>(users);
            return result;
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            try
            {
                var user = await _memberRepo.FindAsync(id);
                if (userDto.Email != user.Email)
                {
                    var userByEmail = await _memberRepo.FindByCondition(x => x.Email.Equals(userDto.Email)).SingleOrDefaultAsync();
                    if (userByEmail != null)
                    {
                        _logger.LogWarning($"Cannot update User {id}: Email {userByEmail.Email} is existed");
                        throw new Exception($"Cannot update User {id}: Email {userByEmail.Email} is existed");
                    }
                }
                var updateUser = _mapper.Map(userDto, user);
                await _memberRepo.UpdateAsync(updateUser);
                _logger.LogInformation($"Update User with id {updateUser.Id} successfully");
                var result = _mapper.Map<UserDto>(user);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw new Exception($"Can not update User with Id {id}");
            }          
        }
    }
}
