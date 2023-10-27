using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Dtos.DepartmentDtos;

public record DepartmentCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile IconFile { get; set; }
    public int? ServiceId { get; set; }
}

public class DepartmentCreateDtoValidator:AbstractValidator<DepartmentCreateDto>
{
    public DepartmentCreateDtoValidator()
    {
        RuleFor(d => d.Name)
            .NotNull()
                .WithMessage("Department name dont be null")
            .NotEmpty()
                 .WithMessage("Department name dont be null");
        RuleFor(d=>d.Description)
            .NotNull()
                .WithMessage("Department Description dont be null")
            .NotEmpty()
                 .WithMessage("Department Description dont be null");
        RuleFor(d => d.IconFile)
            .NotNull()
                .WithMessage("Icon file dont be null")
            .NotEmpty()
                 .WithMessage("Icon file dont be null")
            .SetValidator(new FileValidator());
        RuleFor(d => d.ServiceId)
            .GreaterThan(0)
                .WithMessage("ServiceId must be greater than 0");
    }

}