using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class PatientHistory:BaseEntity
{
    public Recipe? Recipe { get; set; }
    public int? RecipeId { get; set; }
    public Patient Patient { get; set; }
    public string PatientId { get; set; }
    public Doctor Doctor { get; set; }
    public string DoctorId { get; set; }
    public DateTime Date { get; set; }
}


