namespace SerenityHospital.Business.Dtos.PositionDtos;

public record PositionInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}

