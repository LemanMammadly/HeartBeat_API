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
        CreateMap<PatientHistory, PatientListItemDto>().ReverseMap();
        CreateMap<Patient, PatientInfoDto>().ReverseMap();
        CreateMap<Patient, PatientDetailItemDto>().ReverseMap();
        CreateMap<PatientUpdateDto, Patient>().ReverseMap();
        CreateMap<PatientUpdateByAdminDto, Patient>().ReverseMap();
    }
}

