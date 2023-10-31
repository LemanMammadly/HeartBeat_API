using SerenityHospital.Business.Dtos.PatientRoomDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.PatientDtos;

public record PatientInfoDto
{
    public string Id { get; set; }
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
}

