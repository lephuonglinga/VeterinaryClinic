using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Species
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Unit { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
