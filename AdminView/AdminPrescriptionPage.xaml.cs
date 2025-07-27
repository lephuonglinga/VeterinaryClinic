using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VeterinaryClinic.ClientView;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.AdminView
{
    /// <summary>
    /// Interaction logic for AdminPrescriptionPage.xaml
    /// </summary>
    public partial class AdminPrescriptionPage : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        User? currentUser;        
        public AdminPrescriptionPage()
        {
            InitializeComponent();
            currentUser = AdminContext.CurrentAdmin;
            if (currentUser == null)
            {
                MessageBox.Show("No account is currently logged in");
                return;
            }
            Page_Loaded();
        }
        private void Page_Loaded()
        {
            var data = context.Prescriptions
                .Include(p => p.Receipts)
                .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                {
                    PrescriptionId = p.Id,
                    PrescriptionDate = p.Date,
                    PatientId = p != null ? (p.Patient != null ? p.Patient.PatientId : string.Empty) : string.Empty,
                    ReceiptId = r != null ? r.Id : (int?)null,
                    ReceiptAmount = r != null ? r.TotalAmount : 0,
                    DoctorVcn = p != null ? (p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty) : string.Empty,
                    ReceiptDate = r != null ? r.Date : (DateOnly?)null
                })
                .ToList();
            PrescriptionDataGrid.ItemsSource = data;
        }

        private void Presribedmeds_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = AdminWindow.GetInstance();
            if (adminWindow != null)
            {
                Button? button = sender as Button;
                if (button == null)
                {
                    MessageBox.Show("No prescription selected.");
                    return;
                }
                int selectedPresId = (int)button.Tag;
                Models.Prescription? prescription = context.Prescriptions
                    .Include(p => p.Prescribedmeds)
                    .FirstOrDefault(p => p.Id == selectedPresId);

                adminWindow.MainFrame.Navigate(new AdminPrescribedMedPage(prescription));
                adminWindow.Show();
            }
            else
            {
                MessageBox.Show("Please login", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
