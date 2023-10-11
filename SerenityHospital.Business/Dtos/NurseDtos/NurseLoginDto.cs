using FluentValidation;

namespace SerenityHospital.Business.Dtos.NurseDtos;

public record NurseLoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class NurseLoginDtoValidator:AbstractValidator<NurseLoginDto>
{
    public NurseLoginDtoValidator()
    {
        RuleFor(d => d.UserName)
            .NotEmpty()
                .WithMessage("Nurse username dont be empty")
            .NotNull()
                .WithMessage("Nurse username dont be null")
           .MinimumLength(2)
                .WithMessage("Nurse username length must be greater than 2")
           .MaximumLength(45)
                .WithMessage("Nurse username length must be less than 45");
        RuleFor(d => d.Password)
            .NotEmpty()
                .WithMessage("Nurse Password dont be empty")
            .NotNull()
                .WithMessage("Nurse Password dont be null")
           .MinimumLength(6)
                .WithMessage("Nurse password length must be greater than 6");
    }
}