using AutoMapper;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class PatientHistoryMappingProfile:Profile
{
    public PatientHistoryMappingProfile()
    {
        CreateMap<RecipeCreateDto, Recipe>().ReverseMap();
        CreateMap<Recipe, RecipeDetailItemDto>().ReverseMap();
        CreateMap<Recipe, RecipeListItemDto>().ReverseMap();
    }
}

