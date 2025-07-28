using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.DTOs;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Service
{
    public class StatisticService
    {
        private readonly VeterinaryClinicContext db;
        
        public StatisticService()
        {
            db = new VeterinaryClinicContext();
        }

        // 1. Total Income from Prescribed Medications
        public async Task<decimal> GetTotalIncomeAsync()
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Medication != null && pm.Quantity.HasValue)
                .SumAsync(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0));
        }

        // 2. Income by Date Range
        public async Task<decimal> GetIncomeByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Date.HasValue &&
                            pm.Date.Value >= startDate &&
                            pm.Date.Value <= endDate &&
                            pm.Medication != null &&
                            pm.Quantity.HasValue)
                .SumAsync(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0));
        }

        // 3. Monthly Income Report
        public async Task<List<MonthlyIncomeReport>> GetMonthlyIncomeReportAsync(int year)
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Date.HasValue &&
                            pm.Date.Value.Year == year &&
                            pm.Medication != null &&
                            pm.Quantity.HasValue)
                .GroupBy(pm => pm.Date.Value.Month)
                .Select(g => new MonthlyIncomeReport
                {
                    Month = g.Key,
                    TotalIncome = g.Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0)),
                    TotalPrescriptions = g.Count(),
                    Year = year
                })
                .OrderBy(r => r.Month)
                .ToListAsync();
        }

        // 4. Top Revenue Generating Medications
        public async Task<List<MedicationRevenueReport>> GetTopRevenueMedicationsAsync(int topCount = 10)
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Medication != null && pm.Quantity.HasValue)
                .GroupBy(pm => new { pm.MedicationId, pm.Medication.TradeName, pm.Medication.GenericName })
                .Select(g => new MedicationRevenueReport
                {
                    MedicationId = g.Key.MedicationId ?? 0,
                    TradeName = g.Key.TradeName,
                    GenericName = g.Key.GenericName,
                    TotalRevenue = g.Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0)),
                    TotalQuantityPrescribed = g.Sum(pm => pm.Quantity ?? 0),
                    PrescriptionCount = g.Count()
                })
                .OrderByDescending(r => r.TotalRevenue)
                .Take(topCount)
                .ToListAsync();
        }

        // 5. Profit Analysis
        public async Task<List<MedicationProfitReport>> GetMedicationProfitAnalysisAsync()
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Medication != null &&
                            pm.Quantity.HasValue &&
                            pm.Medication.CostPricePerUnit.HasValue &&
                            pm.Medication.SalePricePerUnit.HasValue)
                .GroupBy(pm => new { pm.MedicationId, pm.Medication.TradeName, pm.Medication.GenericName })
                .Select(g => new MedicationProfitReport
                {
                    MedicationId = g.Key.MedicationId ?? 0,
                    TradeName = g.Key.TradeName,
                    GenericName = g.Key.GenericName,
                    TotalRevenue = g.Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0)),
                    TotalCost = g.Sum(pm => (pm.Medication.CostPricePerUnit ?? 0) * (pm.Quantity ?? 0)),
                    TotalProfit = g.Sum(pm => ((pm.Medication.SalePricePerUnit ?? 0) - (pm.Medication.CostPricePerUnit ?? 0)) * (pm.Quantity ?? 0)),
                    ProfitMargin = g.Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0)) > 0 ?
                        (g.Sum(pm => ((pm.Medication.SalePricePerUnit ?? 0) - (pm.Medication.CostPricePerUnit ?? 0)) * (pm.Quantity ?? 0)) /
                         g.Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0))) * 100 : 0
                })
                .OrderByDescending(r => r.TotalProfit)
                .ToListAsync();
        }

        // 6. Revenue by Category
        public async Task<List<CategoryRevenueReport>> GetRevenueByCategoryAsync()
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Medication != null && pm.Quantity.HasValue && !string.IsNullOrEmpty(pm.Medication.Category))
                .GroupBy(pm => pm.Medication.Category)
                .Select(g => new CategoryRevenueReport
                {
                    Category = g.Key,
                    TotalRevenue = g.Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0)),
                    TotalQuantity = g.Sum(pm => pm.Quantity ?? 0),
                    UniqueItems = g.Select(pm => pm.MedicationId).Distinct().Count(),
                    PrescriptionCount = g.Count()
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToListAsync();
        }

        // 7. Average Prescription Value
        public async Task<decimal> GetAveragePrescriptionValueAsync()
        {
            var prescriptionValues = await db.Prescriptions
                .Select(p => p.Prescribedmeds
                    .Where(pm => pm.Medication != null && pm.Quantity.HasValue)
                    .Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0)))
                .Where(value => value > 0)
                .ToListAsync();

            return prescriptionValues.Any() ? prescriptionValues.Average() : 0;
        }

        // 8. Daily Income Summary
        public async Task<List<DailyIncomeReport>> GetDailyIncomeReportAsync(DateOnly startDate, DateOnly endDate)
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Date.HasValue &&
                            pm.Date.Value >= startDate &&
                            pm.Date.Value <= endDate &&
                            pm.Medication != null &&
                            pm.Quantity.HasValue)
                .GroupBy(pm => pm.Date.Value)
                .Select(g => new DailyIncomeReport
                {
                    Date = g.Key,
                    TotalIncome = g.Sum(pm => (pm.Medication.SalePricePerUnit ?? 0) * (pm.Quantity ?? 0)),
                    TotalPrescriptions = g.Count(),
                    UniquePatients = g.Select(pm => pm.Prescription.PatientId).Distinct().Count()
                })
                .OrderBy(r => r.Date)
                .ToListAsync();
        }

        // 9. Expiring Medications Alert
        public async Task<List<ExpiringMedicationReport>> GetExpiringMedicationsAsync(int daysFromNow = 30)
        {
            var cutoffDate = DateOnly.FromDateTime(DateTime.Now.AddDays(daysFromNow));

            return await db.PharmacyInventories
                .Where(pi => pi.ExpirationDate.HasValue && pi.ExpirationDate.Value <= cutoffDate)
                .Select(pi => new ExpiringMedicationReport
                {
                    ItemId = pi.ItemId,
                    TradeName = pi.TradeName,
                    GenericName = pi.GenericName,
                    Category = pi.Category,
                    ExpirationDate = pi.ExpirationDate.Value,
                    DaysUntilExpiry = (pi.ExpirationDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days,
                    CostValue = pi.CostPricePerUnit ?? 0,
                    SaleValue = pi.SalePricePerUnit ?? 0
                })
                .OrderBy(r => r.ExpirationDate)
                .ToListAsync();
        }

        // 10. Most Prescribed Medications
        public async Task<List<MostPrescribedReport>> GetMostPrescribedMedicationsAsync(int topCount = 10)
        {
            return await db.Prescribedmeds
                .Include(pm => pm.Medication)
                .Where(pm => pm.Medication != null)
                .GroupBy(pm => new { pm.MedicationId, pm.Medication.TradeName, pm.Medication.GenericName })
                .Select(g => new MostPrescribedReport
                {
                    MedicationId = g.Key.MedicationId ?? 0,
                    TradeName = g.Key.TradeName,
                    GenericName = g.Key.GenericName,
                    TotalPrescriptions = g.Count(),
                    TotalQuantity = g.Sum(pm => pm.Quantity ?? 0),
                    LastPrescribed = g.Max(pm => pm.Date)
                })
                .OrderByDescending(r => r.TotalPrescriptions)
                .Take(topCount)
                .ToListAsync();
        }
    }
}
