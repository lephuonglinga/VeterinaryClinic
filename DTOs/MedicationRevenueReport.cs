using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class MedicationRevenueReport
    {
        public int MedicationId { get; set; }
        public string TradeName { get; set; }
        public string GenericName { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalQuantityPrescribed { get; set; }
        public int PrescriptionCount { get; set; }
    }
}
