using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.AppoinmentDtos;

public record AppoinmentDetailItemDto
{
    public int Id { get; set; }
    public DoctorInfoDto Doctor { get; set; }
    public PatientDetailItemDto Patient { get; set; }
    public AppoinmentAsDoctorDto AppoinmentAsDoctor { get; set; }
    public string ProblemDesc { get; set; }
    public DateTime AppoinmentDate { get; set; }
    public int Duration { get; set; }
    public AppoinmentStatus Status { get; set; }
    public decimal AppoinmentMoney { get; set; }
    public bool IsDeleted { get; set; }
    public RecipeDetailItemDto Recipe { get; set; }
}

