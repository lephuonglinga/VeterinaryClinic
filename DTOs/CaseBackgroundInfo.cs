using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.DTOs
{
    public class CaseBackgroundInfo
    {
        public int CaseId { get; set; }
        public DateOnly? CaseDate { get; set; }
        public string? CaseStatus { get; set; }
        public string? CaseType { get; set; }
        public string? CaseNotes { get; set; }

        // Diagnosis
        public int? DiagnosisId { get; set; }
        public string? DiagnosisName { get; set; }

        // Patient Info
        public string? PatientId { get; set; }
        public string? PatientSex { get; set; }
        public string? PatientAgeGroup { get; set; }

        // Breed Info
        public int? BreedId { get; set; }
        public string? BreedName { get; set; }

        // Species Info
        public int? SpeciesId { get; set; }
        public string? SpeciesName { get; set; }
    }

}
