using FluentValidation;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Dtos.RecipeDtos;

public record RecipeCreateDto
{
    public int? AppoinmentId { get; set; }
    public string DoctorId { get; set; }
    public string? PatientId { get; set; }
    public string RecipeDesc { get; set; }
}

public class RecipeCreateDtoValidator:AbstractValidator<RecipeCreateDto>
{
    public RecipeCreateDtoValidator()
    {
        RuleFor(r => r.AppoinmentId)
            .GreaterThan(0)
                .WithMessage("AppoinmentId greater than 0");
        RuleFor(r => r.DoctorId)
            .NotEmpty()
            .NotNull()
                .WithMessage("DoctorId can not be null or empty");
        RuleFor(r => r.RecipeDesc)
             .NotEmpty()
             .NotNull()
                 .WithMessage("RecipeDesc can not be null or empty");
    }
}