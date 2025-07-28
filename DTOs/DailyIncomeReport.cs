using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class DailyIncomeReport
    {
        public DateOnly Date { get; set; }
        public decimal TotalIncome { get; set; }
        public int TotalPrescriptions { get; set; }
        public int UniquePatients { get; set; }
    }
}
