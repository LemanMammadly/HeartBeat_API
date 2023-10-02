using FluentValidation;
using Microsoft.AspNetCore.Http;
using SerenityHospital.Business.Validators;

namespace SerenityHospital.Business.Dtos.DepartmentDtos;

public record DepartmentUpdateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile? IconFile { get; set; }
    public int ServiceId { get; set; }
    public IEnumerable<int> PatientRoomIds { get; set; }
}

public class DepartmentUpdateDtoValidator:AbstractValidator<DepartmentUpdateDto>
{
    public DepartmentUpdateDtoValidator()
    {
        RuleFor(d => d.Name)
            .NotNull()
                .WithMessage("Department name dont be null")
            .NotEmpty()
                .WithMessage("Department name dont be null");
        RuleFor(d => d.Description)
            .NotNull()
                .WithMessage("Department Description dont be null")
            .NotEmpty()
                 .WithMessage("Department Description dont be null");
        RuleFor(d => d.IconFile)
            .SetValidator(new FileValidator());
        RuleFor(d => d.ServiceId)
            .NotNull()
                .WithMessage("ServiceId dont be null")
            .NotEmpty()
                 .WithMessage("ServiceId dont be null")
            .GreaterThan(0)
                .WithMessage("ServiceId must be greater than 0");
        RuleFor(d => d.PatientRoomIds)
            .Must(d => IsDistinct(d))
                .WithMessage("You cannot add same patient room ids in same department");
    }

    private bool IsDistinct(IEnumerable<int> ids)
    {
        var encounteredIds = new HashSet<int>();

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
        return true;
    }
}