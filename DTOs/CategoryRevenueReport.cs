using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class CategoryRevenueReport
    {
        public string Category { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalQuantity { get; set; }
        public int UniqueItems { get; set; }
        public int PrescriptionCount { get; set; }
    }
}
