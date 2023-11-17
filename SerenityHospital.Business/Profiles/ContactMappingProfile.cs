using AutoMapper;
using SerenityHospital.Business.Dtos.ContactDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class ContactMappingProfile:Profile
{
    public ContactMappingProfile()
    {
        CreateMap<Contact, ContactListItemDto>().ReverseMap();
        CreateMap<Contact, ContactDetailItemDto>().ReverseMap();
        CreateMap<CreateContactDto, Contact>().ReverseMap();
    }
}

