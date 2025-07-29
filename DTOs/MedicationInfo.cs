using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class MedicationInfo
    {
        public int ItemId { get; set; }
        public string TradeName { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Unit { get; set; }
        public decimal? CostPricePerUnit { get; set; }
        public decimal? SalePricePerUnit { get; set; }
        public string? Barcode { get; set; }
        public DateOnly? ExpirationDate { get; set; }
    }

}
