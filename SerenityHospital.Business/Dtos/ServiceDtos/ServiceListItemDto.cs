using SerenityHospital.Business.Dtos.DepartmentDtos;

namespace SerenityHospital.Business.Dtos.ServiceDtos;

public record ServiceListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ServiceBeginning { get; set; }
    public DateTime ServiceEnding { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<DepartmentListItemDto> Departments { get; set; }
}

