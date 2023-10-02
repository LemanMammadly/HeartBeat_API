using FluentValidation;

namespace SerenityHospital.Business.Dtos.PositionDtos;

public record PositionCreateDto
{
    public string Name { get; set; }
}

public class PositionCreateDtoValidator:AbstractValidator<PositionCreateDto>
{
    public PositionCreateDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
                .WithMessage("Position name dont be empty")
            .NotNull()
                .WithMessage("Position name dont be null");
    }
}

