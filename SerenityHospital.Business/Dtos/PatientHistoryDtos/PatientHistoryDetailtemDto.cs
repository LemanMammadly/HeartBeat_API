using SerenityHospital.Business.Dtos.DoctorDtos;
using SerenityHospital.Business.Dtos.PatientDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Core.Entities;

namespace SerenityHospital.Business.Dtos.PatientHistoryDtos;

public record PatientHistoryDetailtemDto
{
    public int Id { get; set; }
    public string Information { get; set; }
    public DateTime Date { get; set; }
    public RecipeListItemDto Recipe{ get; set; }
}


