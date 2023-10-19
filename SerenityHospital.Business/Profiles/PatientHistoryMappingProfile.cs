using AutoMapper;
using SerenityHospital.Business.Dtos.PatientHistoryDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class PatientHistoryMappingProfile:Profile
{
    public PatientHistoryMappingProfile()
    {
        CreateMap<PatientHistoryCreateDto, PatientHistory>().ReverseMap();
        CreateMap<PatientHistory, PatientHistoryDetailtemDto>().ReverseMap();
        CreateMap<PatientHistory, PatientHistoryListItemDto>().ReverseMap();
    }
}

