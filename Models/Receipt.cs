using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Receipt
{
    public int Id { get; set; }

    public string? PatientId { get; set; }

    public int? PrescriptionId { get; set; }

    public DateOnly? Date { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual Prescription? Prescription { get; set; }
}
