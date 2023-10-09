using FluentValidation;

namespace SerenityHospital.Business.Dtos.PatientDtos;

public record PatientLoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}


public class PatientLoginDtoValidator:AbstractValidator<PatientLoginDto>
{
    public PatientLoginDtoValidator()
    {
        RuleFor(d => d.UserName)
            .NotEmpty()
                .WithMessage("Patient username dont be empty")
            .NotNull()
                .WithMessage("Patient username dont be null")
           .MinimumLength(2)
                .WithMessage("Patient username length must be greater than 2")
           .MaximumLength(45)
                .WithMessage("Patient username length must be less than 45");
        RuleFor(d => d.Password)
            .NotEmpty()
                .WithMessage("Patient Password dont be empty")
            .NotNull()
                .WithMessage("Patient Password dont be null")
           .MinimumLength(6)
                .WithMessage("Patient password length must be greater than 6");
    }
}
