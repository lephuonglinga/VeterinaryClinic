using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VeterinaryClinic.Models;
using VeterinaryClinic.Service;

namespace VeterinaryClinic.AdminView
{
    public partial class AdminDashboardPage : Page
    {
        private readonly StatisticService _statisticService = new StatisticService();
        private User? currentUser;

        public AdminDashboardPage()
        {
            InitializeComponent();
            currentUser = AdminContext.CurrentAdmin;
            if (currentUser == null)
            {
                MessageBox.Show("No account is currently logged in");
                return;
            }
            _ = LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            try
            {
                // Get current year for filtering
                int currentYear = DateTime.Now.Year;
                DateOnly startOfYear = new DateOnly(currentYear, 1, 1);
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                // 1. Load total income
                decimal totalIncome = await _statisticService.GetTotalIncomeAsync();
                TotalIncomeText.Text = totalIncome.ToString("C");

                // 2. Load average prescription value
                decimal avgValue = await _statisticService.GetAveragePrescriptionValueAsync();
                AvgPrescriptionValueText.Text = avgValue.ToString("C");

                // 3. Count of prescriptions this year (from monthly income report)
                var monthlyIncomeReports = await _statisticService.GetMonthlyIncomeReportAsync(currentYear);
                int totalPrescriptionsThisYear = monthlyIncomeReports.Sum(m => m.TotalPrescriptions);
                PrescriptionsCountText.Text = totalPrescriptionsThisYear.ToString();

                // Bind Monthly Income DataGrid
                MonthlyIncomeDataGrid.ItemsSource = monthlyIncomeReports;

                // 4. Expiring medications count (next 30 days)
                var expiringMeds = await _statisticService.GetExpiringMedicationsAsync(30);
                ExpiringMedsCountText.Text = expiringMeds.Count.ToString();
                ExpiringMedicationsDataGrid.ItemsSource = expiringMeds;

                // 5. Top Revenue Medications
                var topMedications = await _statisticService.GetTopRevenueMedicationsAsync(10);
                TopMedicationsDataGrid.ItemsSource = topMedications;

                // 6. Medication Profit Analysis
                var profitAnalysis = await _statisticService.GetMedicationProfitAnalysisAsync();
                MedicationProfitDataGrid.ItemsSource = profitAnalysis;

                // 7. Revenue by Category
                var revenueByCategory = await _statisticService.GetRevenueByCategoryAsync();
                RevenueByCategoryDataGrid.ItemsSource = revenueByCategory;

                // 8. Most Prescribed Medications
                var mostPrescribed = await _statisticService.GetMostPrescribedMedicationsAsync(10);
                MostPrescribedMedicationsDataGrid.ItemsSource = mostPrescribed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
