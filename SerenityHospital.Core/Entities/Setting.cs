using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class Setting:BaseEntity
{
    public string HeaderLogoUrl { get; set; }
    public string FooterLogoUrl { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}


