using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record DoctorUpdateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Description { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public IFormFile? ImageFile { get; set; }
}

public class DoctorUpdateDtoValidator:AbstractValidator<DoctorUpdateDto>
{
    public DoctorUpdateDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
                .WithMessage("Adminstrator name dont be empty")
            .NotNull()
                .WithMessage("Adminstrator name dont be null")
            .MinimumLength(2)
                .WithMessage("Adminstrator name length greater than 2")
            .MaximumLength(25)
                .WithMessage("Adminstrator name length less than 25");
        RuleFor(a => a.Name)
            .NotEmpty()
                .WithMessage("Adminstrator surname dont be empty")
            .NotNull()
                .WithMessage("Adminstrator surname dont be null")
            .MaximumLength(25)
                .WithMessage("Adminstrator surname length less than 25");
        RuleFor(d => d.Email)
            .NotEmpty()
                .WithMessage("Adminstrator email dont be empty")
            .NotNull()
                .WithMessage("Adminstrator email dont be null")
           .Must(u =>
           {
               Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
               var result = regex.Match(u);
               return result.Success;
           })
                 .WithMessage("Please enter valid Adminstrator email");
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
        RuleFor(d => d.Description)
            .NotEmpty()
                .WithMessage("Adminstrator Description dont be empty")
            .NotNull()
                .WithMessage("Adminstrator Description dont be null");
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