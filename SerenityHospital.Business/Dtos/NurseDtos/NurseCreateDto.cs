using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.NurseDtos;

public record NurseCreateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Description { get; set; }
    public decimal Salary { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public IFormFile? ImageFile { get; set; }
    public int DepartmentId { get; set; }
    public DateTime StartWork { get; set; }
    public DateTime EndWork { get; set; }
}

public class NurseCreateDtoValidator:AbstractValidator<NurseCreateDto>
{
    public NurseCreateDtoValidator()
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
        RuleFor(d => d.Description)
            .NotEmpty()
                .WithMessage("Nurse Description dont be empty")
            .NotNull()
                .WithMessage("Nurse Description dont be null");
        RuleFor(d => d.Salary)
            .GreaterThan(500)
                .WithMessage("Nurse salary must be greater than 500");
        RuleFor(d => d.StartWork)
            .NotEmpty()
                .WithMessage("StartWork dont be empty")
            .NotNull()
                .WithMessage("StartWork dont be null");
        RuleFor(d => d.EndWork)
            .NotEmpty()
                .WithMessage("EndWork dont be empty")
            .NotNull()
                .WithMessage("EndWork dont be null");
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
        RuleFor(d => d.DepartmentId)
            .NotEmpty()
                .WithMessage("Nurse DepartmentId dont be empty")
            .NotNull()
                .WithMessage("Nurse DepartmentId dont be null")
            .GreaterThan(0)
                .WithMessage("DepartmentId must be greater than 0");
    }
    private bool BeAValidGender(Gender gender)
    {
        return Enum.IsDefined(typeof(Gender), gender);
    }
}