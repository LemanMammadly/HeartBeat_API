using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class Service:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ServiceBeginning { get; set; }
    public DateTime ServiceEnding { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
}


