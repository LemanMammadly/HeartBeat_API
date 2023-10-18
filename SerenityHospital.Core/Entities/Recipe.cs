using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class Recipe:BaseEntity
{
    public Appoinment? Appoinment { get; set; }
    public int? AppoinmentId { get; set; }
    public Doctor Doctor { get; set; }
    public string DoctorId { get; set; }
    public Patient? Patient { get; set; }
    public string? PatientId { get; set; }
    public PatientHistory PatientHistory { get; set; }
    public string RecipeDesc { get; set; }
}







