using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class GeminiMedicationSuggestionDTO
    {
        public int MedicationId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int? Quantity { get; set; }
        public string? Frequency { get; set; }
        public string? Route { get; set; }
    }

}
