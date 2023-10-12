using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class DoctorRoom:BaseEntity
{
    public int Number { get; set; }
    public Department Department { get; set; }
    public int DepartmentId { get; set; }
    public Doctor Doctor { get; set; }
}


