using FluentValidation;

namespace SerenityHospital.Business.Dtos.PositionDtos;

public record PositionUpdateDto
{
    public string Name { get; set; }
}

public class PositionUpdateDtoValidator:AbstractValidator<PositionUpdateDto>
{
    public PositionUpdateDtoValidator()
    {
        RuleFor(p=>p.Name)
            .NotEmpty()
                .WithMessage("Position name dont be empty")
            .NotNull()
                .WithMessage("Position name dont be null");
    }
}