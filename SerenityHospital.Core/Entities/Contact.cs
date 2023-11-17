using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class Contact:BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
}


