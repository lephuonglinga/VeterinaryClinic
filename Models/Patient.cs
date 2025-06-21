using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Patient
{
    public int Id { get; set; }

    public string PatientId { get; set; } = null!;

    public string? ClientId { get; set; }

    public int? SpeciesId { get; set; }

    public int? BreedId { get; set; }

    public string? Sex { get; set; }

    public string? AgeGroup { get; set; }

    public virtual Breed? Breed { get; set; }

    public virtual ICollection<Case> Cases { get; set; } = new List<Case>();

    public virtual Client? Client { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual Species? Species { get; set; }
}
