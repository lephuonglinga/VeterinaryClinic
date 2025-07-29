using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.DTOs;
using VeterinaryClinic.Models;

public class CaseBackgroundDataService
{
    private readonly VeterinaryClinicContext _context;

    public CaseBackgroundDataService(VeterinaryClinicContext context)
    {
        _context = context;
    }

    public List<CaseBackgroundInfo> GetCaseBackgroundInfos()
    {
        var query = _context.Cases
            .Include(c => c.Diagnosis)
            .Include(c => c.Patient)
                .ThenInclude(p => p.Breed)
            .Include(c => c.Patient)
                .ThenInclude(p => p.Species)
            .AsNoTracking()
            .Select(c => new CaseBackgroundInfo
            {
                CaseId = c.Id,
                CaseDate = c.Date,
                CaseStatus = c.Status,
                CaseType = c.CaseType,
                CaseNotes = c.Notes,

                DiagnosisId = c.DiagnosisId,
                DiagnosisName = c.Diagnosis != null ? c.Diagnosis.Name : null,

                PatientId = c.PatientId,
                PatientSex = c.Patient != null ? c.Patient.Sex : null,
                PatientAgeGroup = c.Patient != null ? c.Patient.AgeGroup : null,

                BreedId = c.Patient != null ? c.Patient.BreedId : null,
                BreedName = (c.Patient != null && c.Patient.Breed != null) ? c.Patient.Breed.Name : null,

                SpeciesId = c.Patient != null ? c.Patient.SpeciesId : null,
                SpeciesName = (c.Patient != null && c.Patient.Species != null) ? c.Patient.Species.Name : null
            });

        return query.ToList();
    }

    public List<CaseBackgroundInfo> GetCasesByPatientId(string patientId)
    {
        var query = _context.Cases
            .Include(c => c.Diagnosis)
            .Include(c => c.Patient)
                .ThenInclude(p => p.Breed)
            .Include(c => c.Patient)
                .ThenInclude(p => p.Species)
            .AsNoTracking()
            .Where(c => c.PatientId == patientId)
            .Select(c => new CaseBackgroundInfo
            {
                CaseId = c.Id,
                CaseDate = c.Date,
                CaseType = c.CaseType,
                CaseNotes = c.Notes,
                CaseStatus = c.Status,

                DiagnosisId = c.DiagnosisId,
                DiagnosisName = c.Diagnosis != null ? c.Diagnosis.Name : null,

                PatientId = c.PatientId,
                PatientSex = c.Patient != null ? c.Patient.Sex : null,
                PatientAgeGroup = c.Patient != null ? c.Patient.AgeGroup : null,

                BreedId = c.Patient != null ? c.Patient.BreedId : null,
                BreedName = (c.Patient != null && c.Patient.Breed != null) ? c.Patient.Breed.Name : null,

                SpeciesId = c.Patient != null ? c.Patient.SpeciesId : null,
                SpeciesName = (c.Patient != null && c.Patient.Species != null) ? c.Patient.Species.Name : null,
            });

        return query.ToList();
    }

}
