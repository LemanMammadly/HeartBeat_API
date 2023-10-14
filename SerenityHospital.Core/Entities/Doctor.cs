using SerenityHospital.Core.Enums;

namespace SerenityHospital.Core.Entities;

public class Doctor:AppUser
{
    public string Description { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public WorkStatus Status { get; set; }
    public DoctorAvailabilityStatus AvailabilityStatus { get; set; }
    public Position Position { get; set; }
    public int PositionId { get; set; }
    public Department Department { get; set; }
    public int DepartmentId { get; set; }
    public DoctorRoom? DoctorRoom { get; set; }
    public int? DoctorRoomId { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Appoinment> Appoinments { get; set; }
}






