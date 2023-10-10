using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record DoctorUpdateByAdminDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Description { get; set; }
    public int Age { get; set; }
    public DateTime StartDate { get; set; }
    public WorkStatus Status { get; set; }
    public decimal Salary { get; set; }
    public Gender Gender { get; set; }
    public IFormFile? ImageFile { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
}

public class DoctorUpdateByAdminDtoValidator:AbstractValidator<DoctorUpdateByAdminDto>
{
    public DoctorUpdateByAdminDtoValidator()
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
        RuleFor(d => d.Password)
            .NotEmpty()
                .WithMessage("Doctor Password dont be empty")
            .NotNull()
                .WithMessage("Doctor Password dont be null")
           .MinimumLength(6)
                .WithMessage("Doctor password length must be greater than 6");
        RuleFor(d => d.Description)
            .NotEmpty()
                .WithMessage("Doctor Description dont be empty")
            .NotNull()
                .WithMessage("Doctor Description dont be null");
        RuleFor(d => d.Salary)
            .GreaterThan(500)
                .WithMessage("Doctor salary must be greater than 500");
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
        RuleFor(d => d.Status)
            .Must(BeAValidStatus)
                .WithMessage("Invalid status");
        RuleFor(d => d.ImageFile)
            .SetValidator(new FileValidator());
        RuleFor(d => d.DepartmentId)
            .NotEmpty()
                .WithMessage("Doctor DepartmentId dont be empty")
            .NotNull()
                .WithMessage("Doctor DepartmentId dont be null")
            .GreaterThan(0)
                .WithMessage("DepartmentId must be greater than 0");
        RuleFor(d => d.PositionId)
            .NotEmpty()
                .WithMessage("Doctor PositionId dont be empty")
            .NotNull()
                .WithMessage("Doctor PositionId dont be null")
            .GreaterThan(0)
                .WithMessage("DepartmentId must be greater than 0");
    }
    private bool BeAValidGender(Gender gender)
    {
        return Enum.IsDefined(typeof(Gender), gender);
    }

    private bool BeAValidStatus(WorkStatus status)
    {
        return Enum.IsDefined(typeof(WorkStatus), status);
    }
}