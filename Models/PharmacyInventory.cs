using System;
using System.Collections.Generic;

namespace VeterinaryClinic.Models;

public partial class PharmacyInventory
{
    public int ItemId { get; set; }

    public string TradeName { get; set; } = null!;

    public string GenericName { get; set; } = null!;

    public string? Category { get; set; }

    public string? Unit { get; set; }

    public decimal? CostPricePerUnit { get; set; }

    public decimal? SalePricePerUnit { get; set; }

    public string? Barcode { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public virtual ICollection<Prescribedmed> Prescribedmeds { get; set; } = new List<Prescribedmed>();
}
