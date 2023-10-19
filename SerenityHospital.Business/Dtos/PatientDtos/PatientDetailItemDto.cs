using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Dtos.PatientHistoryDtos;
using SerenityHospital.Business.Dtos.PatientRoomDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.PatientDtos;

public record PatientDetailItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public Gender Gender { get; set; }
    public BloodType BloodType { get; set; }
    public PatientRoomInfoDto PatientRoom { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public ICollection<AppoinmentInfoDto> Appoinments { get; set; }
    public ICollection<RecipeListItemDto> Recipes { get; set; }
    public ICollection<PatientHistoryListItemDto> PatientHistories { get; set; }
}


