﻿using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Breed
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
