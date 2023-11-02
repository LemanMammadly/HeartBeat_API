using FluentValidation;

namespace SerenityHospital.Business.Dtos.AppoinmentDtos;

public record AppoinmentUpdateDto
{
    public int? DepartmentId { get; set; }
    public string DoctorId { get; set; }
    public string? PatientId { get; set; }
    public string? AppoinmentAsDoctorId { get; set; }
    public string ProblemDesc { get; set; }
    public DateTime AppoinmentDate { get; set; }
    public int Duration { get; set; }
}


public class AppoinmentUpdateDtoValidator:AbstractValidator<AppoinmentUpdateDto>
{
    public AppoinmentUpdateDtoValidator()
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
        RuleFor(a => a.Duration)
            .NotEmpty()
                .WithMessage("Duration dont be empty")
            .NotNull()
                .WithMessage("Duration dont be null")
            .GreaterThan(0)
                        .WithMessage("Duration must be greater than 0")
            .LessThanOrEqualTo(60) 
                        .WithMessage("Duration cannot be greater than 60 minutes"); ;
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