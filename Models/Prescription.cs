using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Prescription
{
    public int Id { get; set; }

    public string? PatientId { get; set; }

    public string? PrescribingDoctorVcnNo { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<Prescribedmed> Prescribedmeds { get; set; } = new List<Prescribedmed>();

    public virtual Doctor? PrescribingDoctorVcnNoNavigation { get; set; }

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
