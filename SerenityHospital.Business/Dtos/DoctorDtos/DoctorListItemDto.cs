using SerenityHospital.Business.Dtos.AppoinmentDtos;
using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.DoctorRoom;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Business.Dtos.RecipeDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record DoctorListItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsDeleted { get; set; }
    public WorkStatus Status { get; set; }
    public PositionInfoDto Position { get; set; }
    public DepartmentInfoDto Department { get; set; }
    public DoctorRoomDetailItemDto DoctorRoom { get; set; }
    public DoctorAvailabilityStatus AvailabilityStatus { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public ICollection<AppoinmentListItemDto> Appoinments { get; set; }
    public ICollection<AppoinmentListItemDto> AppointmentsAsPatient { get; set; }
    public ICollection<RecipeListItemDto> Recipes { get; set; }
}





