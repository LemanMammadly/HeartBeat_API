using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.PatientDtos;

namespace SerenityHospital.Business.Dtos.AppoinmentDtos;

public record AppoinmentInfoDto
{
    public int Id { get; set; }
    public PatientInfoDto Patient { get; set; }
    public string ProblemDesc { get; set; }
    public DateTime AppoinmentDate { get; set; }
}

