using AutoMapper;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class PatientMappingProfile:Profile
{
    public PatientMappingProfile()
    {
        CreateMap<PatientCreateDto, Patient>().ReverseMap();
        CreateMap<Patient, PatientListItemDto>().ReverseMap();
    }
}

