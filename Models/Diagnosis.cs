using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Diagnosis
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
}
