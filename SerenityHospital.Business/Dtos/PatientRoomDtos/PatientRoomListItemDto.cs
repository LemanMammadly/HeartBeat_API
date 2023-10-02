using Microsoft.AspNetCore.Http;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.PatientRoomDtos;

public record PatientRoomListItemDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public PatientRoomType Type { get; set; }
    public PatientRoomStatus Status { get; set; }
    public int Capacity { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int DepartmentId { get; set; }
    public bool IsDeleted { get; set; }
}



