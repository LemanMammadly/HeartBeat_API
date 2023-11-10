﻿using SerenityHospital.Core.Enums;

namespace SerenityHospital.Business.Dtos.DoctorDtos;

public record AppoinmentAsDoctorDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public int Age { get; set; }
    public string? ImageUrl { get; set; }
    public string Email { get; set; }
}


