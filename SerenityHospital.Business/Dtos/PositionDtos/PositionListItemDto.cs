using SerenityHospital.Business.Dtos.DoctorDtos;

namespace SerenityHospital.Business.Dtos.PositionDtos;

public record PositionListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<DoctorInfoDto> Doctors { get; set; }
}


