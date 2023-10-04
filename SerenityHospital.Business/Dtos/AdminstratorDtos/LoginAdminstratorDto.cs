using FluentValidation;

namespace SerenityHospital.Business.Dtos.AdminstratorDtos;

public record LoginAdminstratorDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}


public class LoginAdminstratorDtoValidator:AbstractValidator<LoginAdminstratorDto>
{
    public LoginAdminstratorDtoValidator()
    {
        RuleFor(d => d.UserName)
            .NotEmpty()
                .WithMessage("Adminstrator username dont be empty")
            .NotNull()
                .WithMessage("Adminstrator username dont be null")
           .MinimumLength(2)
                .WithMessage("Adminstrator username length must be greater than 2")
           .MaximumLength(45)
                .WithMessage("Adminstrator username length must be less than 45");
        RuleFor(d => d.Password)
            .NotEmpty()
                .WithMessage("Adminstrator Password dont be empty")
            .NotNull()
                .WithMessage("Adminstrator Password dont be null")
           .MinimumLength(6)
                .WithMessage("Adminstrator password length must be greater than 6");
    }
}