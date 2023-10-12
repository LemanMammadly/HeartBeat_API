using FluentValidation;

namespace SerenityHospital.Business.Dtos.DoctorRoom;

public record DoctorRoomCreateDro
{
    public int Number { get; set; }
    public int DepartmentId { get; set; }
}


public class DoctorRoomCreateDroValidator:AbstractValidator<DoctorRoomCreateDro>
{
    public DoctorRoomCreateDroValidator()
    {
        RuleFor(dr=>dr.Number)
            .NotEmpty()
                .WithMessage("Doctor room number dont be empty")
            .NotNull()
                .WithMessage("Doctor room number dont be null")
            .GreaterThan(0)
                .WithMessage("Doctor Room Number must be greater than 0");
        RuleFor(dr => dr.DepartmentId)
            .NotEmpty()
                .WithMessage("Doctor DepartmentId dont be empty")
            .NotNull()
                .WithMessage("Doctor DepartmentId dont be null")
            .GreaterThan(0)
                .WithMessage("Doctor DepartmentId must be greater than 0");
    }
}