using FluentValidation;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Dtos.PatientHistoryDtos;

public record PatientHistoryCreateDto
{
    public string DoctorId { get; set; }
    public string PatientId { get; set; }
    public string ProblemDesc { get; set; }
    public string RecipeDesc { get; set; }
}

public class PatientHistoryCreateDtoValidator:AbstractValidator<PatientHistoryCreateDto>
{
    public PatientHistoryCreateDtoValidator()
    {
        RuleFor(a => a.DoctorId)
            .NotNull()
                .WithMessage("DoctorId dont be null")
            .NotEmpty()
                 .WithMessage("DoctorId dont be null");
        RuleFor(a => a.PatientId)
            .NotNull()
                .WithMessage("PatientId dont be null")
            .NotEmpty()
                 .WithMessage("PatientId dont be null");
        RuleFor(a => a.ProblemDesc)
            .NotNull()
                .WithMessage("ProblemDesc dont be null")
            .NotEmpty()
                 .WithMessage("ProblemDesc dont be null");
        RuleFor(a => a.RecipeDesc)
            .NotNull()
                .WithMessage("RecipeDesc dont be null")
            .NotEmpty()
                 .WithMessage("RecipeDesc dont be null");
    }
}