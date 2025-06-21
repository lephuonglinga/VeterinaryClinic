using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class PatientDetail
{
    public string? PatientId { get; set; }

    public string? ClientId { get; set; }

    public int? SpeciesId { get; set; }

    public int? BreedId { get; set; }

    public string? Sex { get; set; }

    public string? AgeGroup { get; set; }

    public string? ClientFirstName { get; set; }

    public string? ClientLastName { get; set; }
}
