using AutoMapper;
using User_service.Entities;
using Infrastructure.Extensions;
using Shared.DTOs.UserDTOs;
using EvenBus.Message.IntegrationEvents.Events;

namespace User_service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().IgnoreAllNonExisting();
            CreateMap<EnrolledEvent, UpdateUserDto>().IgnoreAllNonExisting();
        }
    }
}
