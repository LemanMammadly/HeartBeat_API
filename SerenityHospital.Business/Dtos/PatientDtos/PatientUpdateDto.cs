using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.PatientDtos;

public record PatientUpdateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public BloodType BloodType { get; set; }
    public IFormFile? ImageFile { get; set; }
}

public class PatientUpdateDtoValidator:AbstractValidator<PatientUpdateDto>
{
    public PatientUpdateDtoValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
                .WithMessage("Patient name dont be empty")
            .NotNull()
                .WithMessage("Patient name dont be null")
            .MinimumLength(2)
                .WithMessage("Patient name length greater than 2")
            .MaximumLength(25)
                .WithMessage("Patient name length less than 25");
        RuleFor(a => a.Surname)
            .NotEmpty()
                .WithMessage("Patient surname dont be empty")
            .NotNull()
                .WithMessage("Patient surname dont be null")
            .MaximumLength(25)
                .WithMessage("Patient surname length less than 25");
        RuleFor(d => d.Email)
            .NotEmpty()
                .WithMessage("Patient email dont be empty")
            .NotNull()
                .WithMessage("Patient email dont be null")
           .Must(u =>
           {
               Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
               var result = regex.Match(u);
               return result.Success;
           })
                 .WithMessage("Please enter valid Patient email");
        RuleFor(d => d.UserName)
            .NotEmpty()
                .WithMessage("Patient username dont be empty")
            .NotNull()
                .WithMessage("Patient username dont be null")
           .MinimumLength(2)
                .WithMessage("Patient username length must be greater than 2")
           .MaximumLength(45)
                .WithMessage("Patient username length must be less than 45");
        RuleFor(s => s.PhoneNumber)
            .NotEmpty()
                .WithMessage("Phone number dont be empty")
            .NotNull()
                .WithMessage("Phone number dont be null")
            .Must(s =>
            {
                Regex regex = new Regex(@"^(\+994|0)(50|51|55|70|77|99)[1-9]\d{6}$");
                var result = regex.Match(s);
                return result.Success;
            })
            .WithMessage("Please enter valid phone number");
        RuleFor(a => a.Address)
            .NotEmpty()
                .WithMessage("Patient surname dont be empty")
            .NotNull()
                .WithMessage("Patient surname dont be null");
        RuleFor(d => d.Age)
            .NotEmpty()
                .WithMessage("Patient ager dont be empty")
            .NotNull()
                .WithMessage("Patient age dont be null");
        RuleFor(d => d.Gender)
            .Must(BeAValidGender)
                .WithMessage("Invalid gender");
        RuleFor(d => d.BloodType)
            .Must(BeAValidBloodType)
                .WithMessage("Invalid BloodType");
        RuleFor(d => d.ImageFile)
            .SetValidator(new FileValidator());
    }
    private bool BeAValidGender(Gender gender)
    {
        return Enum.IsDefined(typeof(Gender), gender);
    }
    private bool BeAValidBloodType(BloodType bloodType)
    {
        return Enum.IsDefined(typeof(BloodType), bloodType);
    }
}