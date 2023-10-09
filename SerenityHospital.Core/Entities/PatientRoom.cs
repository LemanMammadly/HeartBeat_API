using SerenityHospital.Core.Entities.Common;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Core.Entities;

public class PatientRoom:BaseEntity
{
    public int Number { get; set; }
    public PatientRoomType Type { get; set; }
    public PatientRoomStatus Status { get; set; }
    public int Capacity { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public Department Department { get; set; }
    public int DepartmentId { get; set; }
    public IEnumerable<Patient> Patients { get; set; }
}





