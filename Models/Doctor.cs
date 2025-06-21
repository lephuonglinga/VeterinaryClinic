using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Doctor
{
    public string Username { get; set; } = null!;

    public string VcnNo { get; set; } = null!;

    public string? Email { get; set; }

    public int? YearGraduated { get; set; }

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual User UsernameNavigation { get; set; } = null!;
}
