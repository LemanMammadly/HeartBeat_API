﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.PatientRoomDtos;

public record PatientRoomUpdateDto
{
    public int Number { get; set; }
    public PatientRoomType Type { get; set; }
    public int Capacity { get; set; }
    public decimal Price { get; set; }
    public IFormFile? ImageFile { get; set; }
    public int? DepartmentId { get; set; }
    public IEnumerable<string>? Patientids { get; set; }
}

public class PatientRoomUpdateDtoValidator:AbstractValidator<PatientRoomUpdateDto>
{
    public PatientRoomUpdateDtoValidator()
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
            .SetValidator(new FileValidator());
        RuleFor(pr => pr.DepartmentId)
            .GreaterThan(0)
                .WithMessage("DepartmentId must be greater than 0");
        RuleFor(p => p.Patientids)
            .Must(p => IsDistinct(p))
                .WithMessage("You cannot add same patient id in same room");
    }

    private bool BeAValidPatientRoomType(PatientRoomType type)
    {
        return Enum.IsDefined(typeof(PatientRoomType), type);
    }
    private bool IsDistinct(IEnumerable<string> ids)
    {
        var encounteredIds = new HashSet<string>();

        if(ids !=null)
        {
            foreach (var id in ids)
            {
                if (!encounteredIds.Contains(id))
                {
                    encounteredIds.Add(id);
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
}