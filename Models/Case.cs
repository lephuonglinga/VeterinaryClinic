using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Case
{
    public int Id { get; set; }

    public string? PatientId { get; set; }

    public DateOnly? Date { get; set; }

    public string? Status { get; set; }

    public int? DiagnosisId { get; set; }

    public string? CaseType { get; set; }

    public string? Notes { get; set; }

    public string? DoctorVcnNo { get; set; }

    public virtual Diagnosis? Diagnosis { get; set; }

    public virtual Doctor? DoctorVcnNoNavigation { get; set; }

    public virtual Patient? Patient { get; set; }
}
