using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.AdminDtos;

public record AdminCreateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public Gender Gender { get; set; }
    public IFormFile? ImageFile { get; set; }
}


public class AdminCreateDtoValidator:AbstractValidator<AdminCreateDto>
{
    public AdminCreateDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
                .WithMessage("Admin name dont be empty")
            .NotNull()
                .WithMessage("Admin name dont be null")
            .MinimumLength(2)
                .WithMessage("Admin name length greater than 2")
            .MaximumLength(25)
                .WithMessage("Admin name length less than 25");
         RuleFor(a => a.Surname)
            .NotEmpty()
                .WithMessage("Admin surname dont be empty")
            .NotNull()
                .WithMessage("Admin surname dont be null")
            .MaximumLength(25)
                .WithMessage("Admin surname length less than 25");
        RuleFor(d => d.Email)
            .NotEmpty()
                .WithMessage("Admin email dont be empty")
            .NotNull()
                .WithMessage("Admin email dont be null")
           .Must(u =>
           {
               Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
               var result = regex.Match(u);
               return result.Success;
           })
                 .WithMessage("Please enter valid Admin email");
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
        RuleFor(d => d.Age)
            .GreaterThan(18)
                .WithMessage("Adminstrator age must be greater than 18");
        RuleFor(d => d.Gender)
            .Must(BeAValidGender)
                .WithMessage("Invalid gender");
        RuleFor(d => d.ImageFile)
            .SetValidator(new FileValidator());
    }

    private bool BeAValidGender(Gender gender)
    {
        return Enum.IsDefined(typeof(Gender), gender);
    }
}