using FluentValidation;

namespace SerenityHospital.Business.Dtos.DoctorRoom;

public record DoctorRoomUpdateDto
{
    public int Number { get; set; }
    public int? DepartmentId { get; set; }
    public string? DoctorId { get; set; }
}

public class DoctorRoomUpdateDtoValidator:AbstractValidator<DoctorRoomUpdateDto>
{
    public DoctorRoomUpdateDtoValidator()
    {
        RuleFor(dr => dr.Number)
            .NotEmpty()
                .WithMessage("Doctor room number dont be empty")
            .NotNull()
                .WithMessage("Doctor room number dont be null")
            .GreaterThan(0)
                .WithMessage("Doctor Room Number must be greater than 0");
        RuleFor(dr => dr.DepartmentId)
            .GreaterThan(0)
                .WithMessage("Doctor DepartmentId must be greater than 0");
    }
}