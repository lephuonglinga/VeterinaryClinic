using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class MonthlyIncomeReport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public int TotalPrescriptions { get; set; }
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM");
    }
}
