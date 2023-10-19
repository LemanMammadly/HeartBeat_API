
using SerenityHospital.Business.Dtos.RecipeDtos;

namespace SerenityHospital.Business.Services.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeListItemDto>> GetAllAsync(bool takeAll);
    Task<RecipeDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(RecipeCreateDto dto);
    Task UpdateAsync(int id, RecipeUpdateDto dto);
}

