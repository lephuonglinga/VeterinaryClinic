using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.DTOs;
using VeterinaryClinic.Models;

public class PharmacyInventoryDataService
{
    private readonly VeterinaryClinicContext _context;

    public PharmacyInventoryDataService(VeterinaryClinicContext context)
    {
        _context = context;
    }

    public List<MedicationInfo> GetAllMedications()
    {
        var query = _context.PharmacyInventories
            .AsNoTracking()
            .Select(med => new MedicationInfo
            {
                ItemId = med.ItemId,
                TradeName = med.TradeName,
                GenericName = med.GenericName,
                Category = med.Category,
                Unit = med.Unit,
                CostPricePerUnit = med.CostPricePerUnit,
                SalePricePerUnit = med.SalePricePerUnit,
                Barcode = med.Barcode,
                ExpirationDate = med.ExpirationDate
            });

        return query.ToList();
    }
    public List<MedicationInfo> GetAvailableMedicationsExcludingPatientCurrent(string patientId)
    {
        // Get medication IDs already prescribed to patient
        var prescribedMedicationIds = _context.Prescribedmeds
            .Where(pm => pm.Prescription != null &&
                         pm.Prescription.PatientId == patientId &&
                         pm.Medication != null)
            .Select(pm => pm.Medication.ItemId)
            .Distinct()
            .ToList();

        // Return medications from inventory except those already prescribed
        var query = _context.PharmacyInventories
            .Where(med => !prescribedMedicationIds.Contains(med.ItemId))
            .AsNoTracking()
            .Select(med => new MedicationInfo
            {
                ItemId = med.ItemId,
                TradeName = med.TradeName,
                GenericName = med.GenericName,
                Category = med.Category,
                Unit = med.Unit,
                CostPricePerUnit = med.CostPricePerUnit,
                SalePricePerUnit = med.SalePricePerUnit,
                Barcode = med.Barcode,
                ExpirationDate = med.ExpirationDate
            });

        return query.ToList();
    }
    public List<PharmacyInventory> GetAvailablePharmacyInventoriesExcluding(string patientId)
    {
        var prescribedMedicationIds = _context.Prescribedmeds
            .Where(pm => pm.Prescription != null &&
                         pm.Prescription.PatientId == patientId &&
                         pm.Medication != null)
            .Select(pm => pm.Medication.ItemId)
            .Distinct()
            .ToList();

        return _context.PharmacyInventories
            .Where(pi => !prescribedMedicationIds.Contains(pi.ItemId))
            .AsNoTracking()
            .ToList();
    }

}
