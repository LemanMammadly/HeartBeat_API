using FluentValidation;

namespace SerenityHospital.Business.Dtos.AppoinmentDtos;

public record AppoinmentCreateDto
{
    public string DoctorId { get; set; }
    public string ProblemDesc { get; set; }
    public DateTime AppoinmentDate { get; set; }
}

public class AppoinmentCreateDtoValidator:AbstractValidator<AppoinmentCreateDto>
{
    public AppoinmentCreateDtoValidator()
    {
        RuleFor(a => a.DoctorId)
            .NotNull()
                .WithMessage("DoctorId dont be null")
            .NotEmpty()
                 .WithMessage("DoctorId dont be null");
        RuleFor(a => a.AppoinmentDate)
            .NotEmpty()
                .WithMessage("AppoinmentDate dont be empty")
            .NotNull()
                .WithMessage("AppoinmentDate dont be null")
            .Must(BeValidAppointmentTime)
            .WithMessage("Invalid appointment time. Appointments are only available between 09:00 and 18:00 on weekdays.");
    }
    
    private bool BeValidAppointmentTime(DateTime appoinmentDate)
    {
        TimeSpan validStartTime = new TimeSpan(9, 0, 0);
        TimeSpan validEndTime = new TimeSpan(18, 0, 0);

        if (appoinmentDate.DayOfWeek >= DayOfWeek.Monday && appoinmentDate.DayOfWeek <= DayOfWeek.Friday)
        {
            TimeSpan appointmentTime = appoinmentDate.TimeOfDay;

            return (appointmentTime >= validStartTime) && (appointmentTime <= validEndTime);
        }

        return false;
    }
}