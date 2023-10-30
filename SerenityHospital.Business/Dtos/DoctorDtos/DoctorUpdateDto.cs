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
                .WithMessage("Doctor name dont be empty")
            .NotNull()
                .WithMessage("Doctor name dont be null")
            .MinimumLength(2)
                .WithMessage("Doctor name length greater than 2")
            .MaximumLength(25)
                .WithMessage("Doctor name length less than 25");
        RuleFor(a => a.Surname)
            .NotEmpty()
                .WithMessage("Doctor surname dont be empty")
            .NotNull()
                .WithMessage("Doctor surname dont be null")
            .MaximumLength(25)
                .WithMessage("Doctor surname length less than 25");
        RuleFor(d => d.Email)
            .NotEmpty()
                .WithMessage("Doctor email dont be empty")
            .NotNull()
                .WithMessage("Doctor email dont be null")
           .Must(u =>
           {
               Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
               var result = regex.Match(u);
               return result.Success;
           })
                 .WithMessage("Please enter valid Doctor email");
        RuleFor(d => d.UserName)
            .NotEmpty()
                .WithMessage("Doctor username dont be empty")
            .NotNull()
                .WithMessage("Doctor username dont be null")
           .MinimumLength(2)
                .WithMessage("Doctor username length must be greater than 2")
           .MaximumLength(45)
                .WithMessage("Doctor username length must be less than 45");
        RuleFor(d => d.Description)
            .NotEmpty()
                .WithMessage("Doctor Description dont be empty")
            .NotNull()
                .WithMessage("Doctor Description dont be null");
        RuleFor(d => d.Age)
            .NotEmpty()
                .WithMessage("Doctor ager dont be empty")
            .NotNull()
                .WithMessage("Doctor ager dont be null")
            .GreaterThan(18)
                .WithMessage("Doctor age must be greater than 18");
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