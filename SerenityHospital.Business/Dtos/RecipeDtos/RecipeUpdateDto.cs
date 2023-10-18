using FluentValidation;

namespace SerenityHospital.Business.Dtos.RecipeDtos;

public record RecipeUpdateDto
{
    public string RecipeDesc { get; set; }
}

public class RecipeUpdateDtoValidator : AbstractValidator<RecipeUpdateDto>
{
    public RecipeUpdateDtoValidator()
    {
        RuleFor(r => r.RecipeDesc)
             .NotEmpty()
             .NotNull()
                 .WithMessage("RecipeDesc can not be null or empty");
    }
}