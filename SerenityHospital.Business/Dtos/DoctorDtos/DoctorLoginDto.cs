using FluentValidation;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record DoctorLoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class DoctorLoginDtoValidator:AbstractValidator<DoctorLoginDto>
{
    public DoctorLoginDtoValidator()
    {
        RuleFor(d => d.UserName)
            .NotEmpty()
                .WithMessage("Doctor username dont be empty")
            .NotNull()
                .WithMessage("Doctor username dont be null")
           .MinimumLength(2)
                .WithMessage("Doctor username length must be greater than 2")
           .MaximumLength(45)
                .WithMessage("Doctor username length must be less than 45");
        RuleFor(d => d.Password)
            .NotEmpty()
                .WithMessage("Doctor Password dont be empty")
            .NotNull()
                .WithMessage("Doctor Password dont be null")
           .MinimumLength(6)
                .WithMessage("Doctor password length must be greater than 6");
    }
}