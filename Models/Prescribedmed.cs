using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class Prescribedmed
{
    public int Id { get; set; }

    public int? PrescriptionId { get; set; }

    public int? MedicationId { get; set; }

    public int? Quantity { get; set; }

    public string? Frequency { get; set; }

    public string? Route { get; set; }

    public DateOnly? Date { get; set; }

    public virtual PharmacyInventory? Medication { get; set; }
    
    public decimal? TotalPrice
    {
        get { return Medication?.SalePricePerUnit * Quantity; }        
    }


    public virtual Prescription? Prescription { get; set; }
}
