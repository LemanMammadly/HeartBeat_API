﻿using Microsoft.AspNetCore.Identity;
using System.Numerics;
using SerenityHospital.Core.Enums;

namespace SerenityHospital.Core.Entities;

public class Patient:AppUser
{
    public BloodType BloodType { get; set; }
    public string Address { get; set; }
    public PatientRoom? PatientRoom { get; set; }
    public int? PatientRoomId { get; set; }
    public ICollection<Appoinment> Appoinments { get; set; }
    public ICollection<Recipe> Recipes { get; set; }
    public ICollection<PatientHistory> PatientHistories { get; set; }

}



