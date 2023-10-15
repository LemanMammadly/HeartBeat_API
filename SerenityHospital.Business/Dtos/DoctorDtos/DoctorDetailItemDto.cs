﻿using SerenityHospital.Business.Dtos.DepartmentDtos;
using SerenityHospital.Business.Dtos.DoctorRoom;
using SerenityHospital.Business.Dtos.PositionDtos;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record DoctorDetailItemDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsDeleted { get; set; }
    public PositionInfoDto Position { get; set; }
    public DepartmentInfoDto Department { get; set; }
    public DoctorRoomDetailItemDto DoctorRoom { get; set; }
    public DoctorAvailabilityStatus AvailabilityStatus { get; set; }
    public IEnumerable<string> Roles { get; set; }
}

