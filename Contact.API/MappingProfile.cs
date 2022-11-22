using AutoMapper;
using Contact.API.DTOs;
using Contact.API.Entities;
using Infrastructure.Extensions;

namespace Contact.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogContact, ContactDto>().ReverseMap();
            CreateMap<CreateContactDto, CatalogContact>().ReverseMap();
            CreateMap<UpdateContactDto, CatalogContact>().IgnoreAllNonExisting();
        }
    }
}
