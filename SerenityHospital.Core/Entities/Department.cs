using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class Department:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public Service Service { get; set; }
    public int ServiceId { get; set; }
    public ICollection<PatientRoom> PatientRooms { get; set; }
    public ICollection<Doctor> Doctors { get; set; }
}



