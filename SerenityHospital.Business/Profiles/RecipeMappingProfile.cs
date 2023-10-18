using AutoMapper;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Profiles;

public class RecipeMappingProfile:Profile
{
    public RecipeMappingProfile()
    {
        CreateMap<RecipeCreateDto, Recipe>().ReverseMap();
        CreateMap<RecipeUpdateDto, Recipe>().ReverseMap();
        CreateMap<Recipe, RecipeDetailItemDto>().ReverseMap();
        CreateMap<Recipe, RecipeListItemDto>().ReverseMap();
        CreateMap<PatientHistory, RecipeListItemDto>().ReverseMap();
    }
}

