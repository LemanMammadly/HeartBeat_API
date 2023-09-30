using FluentValidation;

namespace SerenityHospital.Business.Dtos.HospitalDtos;

public record HospitalCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class HospitalCreateDtoValidator:AbstractValidator<HospitalCreateDto>
{
    public HospitalCreateDtoValidator()
    {
        RuleFor(h => h.Name)
            .NotEmpty()
                .WithMessage("Name field dont be empty")
            .NotNull()
                .WithMessage("Name field dont be null")
            .MaximumLength(64)
                .WithMessage("Name field length must be less than 64");
        RuleFor(h => h.Description)
             .NotEmpty()
                .WithMessage("Description field dont be empty")
            .NotNull()
                .WithMessage("Description field dont be null");
    }
}