using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class MostPrescribedReport
    {
        public int MedicationId { get; set; }
        public string TradeName { get; set; }
        public string GenericName { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalQuantity { get; set; }
        public DateOnly? LastPrescribed { get; set; }
    }
}
