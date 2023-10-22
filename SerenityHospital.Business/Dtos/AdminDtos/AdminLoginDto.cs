using FluentValidation;

namespace SerenityHospital.Business.Dtos.AdminDtos;

public record AdminLoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class AdminLoginDtoValidator:AbstractValidator<AdminLoginDto>
{
    public AdminLoginDtoValidator()
    {
        RuleFor(d => d.UserName)
            .NotEmpty()
                .WithMessage("Admin username dont be empty")
            .NotNull()
                .WithMessage("Admin username dont be null")
           .MinimumLength(2)
                .WithMessage("Admin username length must be greater than 2")
           .MaximumLength(45)
                .WithMessage("Admin username length must be less than 45");
        RuleFor(d => d.Password)
            .NotEmpty()
                .WithMessage("Admin Password dont be empty")
            .NotNull()
                .WithMessage("Admin Password dont be null")
           .MinimumLength(6)
                .WithMessage("Admin password length must be greater than 6");
    }
}
