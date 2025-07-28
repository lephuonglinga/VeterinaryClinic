using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class ExpiringMedicationReport
    {
        public int ItemId { get; set; }
        public string TradeName { get; set; }
        public string GenericName { get; set; }
        public string Category { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public decimal CostValue { get; set; }
        public decimal SaleValue { get; set; }
    }
}
