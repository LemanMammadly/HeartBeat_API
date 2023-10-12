﻿using AutoMapper;
using SerenityHospital.Business.Dtos.NurseDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class NurseMappingProfile:Profile
{
    public NurseMappingProfile()
    {
        CreateMap<NurseCreateDto, Nurse>().ReverseMap();
        CreateMap<NurseUpdateDto, Nurse>().ReverseMap();
        CreateMap<NurseUpdateByAdminDto, Nurse>().ReverseMap();
        CreateMap<Nurse, NurseListItemDto>().ReverseMap();
        CreateMap<Nurse, NurseDetailItemDto>().ReverseMap();
    }
}

