using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.NurseDtos;

public record NurseUpdateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public IFormFile? ImageFile { get; set; }
}

public class NurseUpdateDtoValidator:AbstractValidator<NurseUpdateDto>
{
    public NurseUpdateDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
                .WithMessage("Nurse name dont be empty")
            .NotNull()
                .WithMessage("Nurse name dont be null")
            .MinimumLength(2)
                .WithMessage("Nurse name length greater than 2")
            .MaximumLength(25)
                .WithMessage("Nurse name length less than 25");
        RuleFor(a => a.Surname)
            .NotEmpty()
                .WithMessage("Nurse surname dont be empty")
            .NotNull()
                .WithMessage("Nurse surname dont be null")
            .MaximumLength(25)
                .WithMessage("Nurse surname length less than 25");
        RuleFor(d => d.Email)
            .NotEmpty()
                .WithMessage("Nurse email dont be empty")
            .NotNull()
                .WithMessage("Nurse email dont be null")
           .Must(u =>
           {
               Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
               var result = regex.Match(u);
               return result.Success;
           })
                 .WithMessage("Please enter valid Nurse email");
        RuleFor(d => d.Age)
            .NotEmpty()
                .WithMessage("Nurse ager dont be empty")
            .NotNull()
                .WithMessage("Nurse ager dont be null")
            .GreaterThan(18)
                .WithMessage("Nurse age must be greater than 18");
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
