using AutoMapper;
using User_service.Entities;
using Infrastructure.Extensions;
using Infrastructure.DTOs.UserDTOs;

namespace User_service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().IgnoreAllNonExisting();
        }
    }
}
