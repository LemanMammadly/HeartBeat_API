using SerenityHospital.Core.Entities;
using SerenityHospital.DAL.Contexts;
using SerenityHospital.DAL.Repositories.Interfaces;

namespace SerenityHospital.DAL.Repositories.Implements;

public class RecipeRepository : Repository<Recipe>, IRecipeRepository
{
    public RecipeRepository(AppDbContext context) : base(context)
    {
    }
}

