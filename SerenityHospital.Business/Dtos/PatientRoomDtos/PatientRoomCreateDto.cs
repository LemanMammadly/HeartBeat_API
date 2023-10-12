using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Entities;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.PatientRoomDtos;

public record PatientRoomCreateDto
{
    public int Number { get; set; }
    public PatientRoomType Type { get; set; }
    public int Capacity { get; set; }
    public decimal Price { get; set; }
    public IFormFile ImageFile { get; set; }
    public int DepartmentId { get; set; }
}


public class PatientRoomCreateDtoValidator:AbstractValidator<PatientRoomCreateDto>
{
    public PatientRoomCreateDtoValidator()
    {
        RuleFor(pr => pr.Number)
            .NotEmpty()
                .WithMessage("Patient room number dont be empty")
            .NotNull()
                .WithMessage("Patient room number dont be null")
            .GreaterThan(0)
                .WithMessage("Patient Room Number must be greater than 0");
        RuleFor(pr => pr.Type)
            .NotEmpty()
                .WithMessage("Patient room type dont be empty")
            .NotNull()
                .WithMessage("Patient room type dont be null")
            .Must(BeAValidPatientRoomType)
                .WithMessage("Enter valid PatientRoomType");
        RuleFor(pr => pr.Capacity)
            .NotEmpty()
                .WithMessage("Patient room capacity dont be empty")
            .NotNull()
                .WithMessage("Patient room capacity dont be null")
            .GreaterThan(0)
                .WithMessage("Patient Room Capacity must be greater than 0")
            .LessThan(4)
                .WithMessage("Patient Room Capacity must be less than 4");
        RuleFor(pr => pr.Price)
            .NotEmpty()
                .WithMessage("Patient room price dont be empty")
            .NotNull()
                .WithMessage("Patient room price dont be null")
            .GreaterThan(0)
                .WithMessage("Patient Room Price must be greater than 0");
        RuleFor(pr => pr.ImageFile)
            .NotNull()
                .WithMessage("Icon file dont be null")
            .NotEmpty()
                 .WithMessage("Icon file dont be null")
            .SetValidator(new FileValidator());
        RuleFor(pr=>pr.DepartmentId)
            .NotNull()
                .WithMessage("DepartmentId dont be null")
            .NotEmpty()
                 .WithMessage("DepartmentId dont be null")
            .GreaterThan(0)
                .WithMessage("DepartmentId must be greater than 0");
    }

    private bool BeAValidPatientRoomType(PatientRoomType type)
    {
        return Enum.IsDefined(typeof(PatientRoomType), type);
    }

    private bool BeAValidStatus(PatientRoomStatus status)
    {
        return Enum.IsDefined(typeof(PatientRoomStatus), status);
    }
}