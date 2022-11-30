using AutoMapper;
using Course_service.Entities;
using EvenBus.Message.IntegrationEvents.Events;
using Infrastructure.Extensions;
using Shared.DTOs.CourseDTOs;
using Shared.DTOs.EnrollmentDTOs;
using Shared.DTOs.UserDTOs;

namespace Course_service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
            CreateMap<CreateCourseDto, Course>().ReverseMap();
            CreateMap<UpdateUserDto, UserDto>().ReverseMap();
            CreateMap<CreateEnrollmentDto, Enrollment>().ReverseMap();
            CreateMap<EnrolledEvent, UserDto>().ReverseMap();
            CreateMap<UpdateCourseDto, Course>().IgnoreAllNonExisting();
        }
    }
}
