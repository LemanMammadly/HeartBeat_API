using SerenityHospital.Core.Entities.Common;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Core.Entities;

public class Appoinment:BaseEntity
{
    public Doctor Doctor { get; set; }
    public string DoctorId { get; set; }
    public Patient Patient { get; set; }
    public string PatientId { get; set; }
    public string ProblemDesc { get; set; }
    public DateTime AppoinmentDate { get; set; }
    public int Duration { get; set; }
    public AppoinmentStatus Status { get; set; }
    public decimal AppoinmentMoney { get; set; }
}




