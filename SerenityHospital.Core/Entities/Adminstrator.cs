using SerenityHospital.Core.Enums;

namespace SerenityHospital.Core.Entities;

public class Adminstrator:AppUser
{
    public string Description { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public WorkStatus Status { get; set; }
    public Hospital Hospital { get; set; }
    public int HospitalId { get; set; }
}





