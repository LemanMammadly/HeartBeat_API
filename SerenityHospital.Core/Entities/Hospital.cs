using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class Hospital:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Adminstrator Adminstrator { get; set; }
}



