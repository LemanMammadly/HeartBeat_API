﻿using SerenityHospital.Core.Entities.Common;

namespace SerenityHospital.Core.Entities;

public class Position:BaseEntity
{
    public string Name { get; set; }
    public ICollection<Doctor> Doctors { get; set; }
}



