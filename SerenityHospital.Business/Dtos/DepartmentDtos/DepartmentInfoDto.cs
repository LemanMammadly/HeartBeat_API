namespace SerenityHospital.Business.Dtos.DepartmentDtos;

public record DepartmentInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}


