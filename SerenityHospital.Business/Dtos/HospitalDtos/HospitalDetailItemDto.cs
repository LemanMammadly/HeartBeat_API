using SerenityHospital.Business.Dtos.AdminstratorDtos;

namespace SerenityHospital.Business.Dtos.HospitalDtos;

public record HospitalDetailItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
    public AdminstratorDetailItemDto Adminstrator { get; set; }
}




