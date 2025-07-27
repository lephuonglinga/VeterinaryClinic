using Azure;
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
using VeterinaryClinic.AdminView;
using VeterinaryClinic.DoctorView;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.ClientView
{
    /// <summary>
    /// Interaction logic for ClientPrescriptionPage.xaml
    /// </summary>
    public partial class ClientPrescriptionPage : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        Patient? currentPatient;
        Client? currentClient = ClientContext.CurrentClient;
        public ClientPrescriptionPage(Patient? patient)
        {            
            if (currentClient == null)
            {
                MessageBox.Show("No account is currently logged in");
                return;
            }
            currentPatient = patient;
            InitializeComponent();
            Page_Loaded();
        }

        private void Page_Loaded()
        {
            if (currentPatient == null)
            {
                // Create a list of PrescriptionViewModel objects instead of anonymous objects
                var data = context.Prescriptions
                    .Include(p => p.Receipts)
                    .Include(p => p.Patient)
                    .Include(p => p.PrescribingDoctorVcnNoNavigation)
                    .Where(p => p.Patient.ClientId == currentClient.ClientId)
                    .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                    {
                        IsSelected = false,
                        PrescriptionId = p.Id,
                        PrescriptionDate = p.Date,
                        PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                        ReceiptId = r != null ? r.Id : (int?)null,
                        ReceiptAmount = (decimal)(r != null ? r.TotalAmount : 0),
                        DoctorVcn = p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty,
                        PaymentMethod = r != null ? r.PaymentMethod : string.Empty
                    })
                    .ToList();
                dgPres.ItemsSource = data;
            }
            else
            {
                var data = context.Prescriptions
                    .Include(p => p.Receipts)
                    .Include(p => p.Patient)
                    .Include(p => p.PrescribingDoctorVcnNoNavigation)
                    .Where(p => p.PatientId == currentPatient.PatientId && p.Patient.ClientId.Equals(currentClient.ClientId)) // Filter by current patient
                    .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                    {
                        IsSelected = false,
                        PrescriptionId = p.Id,
                        PrescriptionDate = p.Date,
                        PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                        ReceiptId = r != null ? r.Id : (int?)null,
                        ReceiptAmount = (decimal)(r != null ? r.TotalAmount : 0),
                        DoctorVcn = p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty,
                        PaymentMethod = r != null ? r.PaymentMethod : string.Empty
                    })
                    .ToList();
                dgPres.ItemsSource = data;
            }
        }
        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void Presribedmeds_Click(object sender, RoutedEventArgs e)
        {
            ClientDetailsWindow clientDetailsWindow = ClientDetailsWindow.GetInstance();
            if (clientDetailsWindow != null)
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

                clientDetailsWindow.MainFrame.Navigate(new ClientPrescribedmedPage(prescription));
                clientDetailsWindow.Show();
            }
            else
            {
                MessageBox.Show("Please login", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                Page_Loaded();
                return;
            }

            if (currentPatient == null)
            {
                var data = context.Prescriptions
                    .Include(p => p.Receipts)
                    .Include(p => p.Patient)
                    .Include(p => p.PrescribingDoctorVcnNoNavigation)
                    .Where(p => p.Patient.ClientId == currentClient.ClientId &&
                                (p.Id.ToString().Contains(searchText) ||
                                 p.PrescribingDoctorVcnNoNavigation.VcnNo.ToLower().Contains(searchText)))
                    .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                    {
                        IsSelected = false,
                        PrescriptionId = p.Id,
                        PrescriptionDate = p.Date,
                        PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                        ReceiptId = r != null ? r.Id : (int?)null,
                        ReceiptAmount = (decimal)(r != null ? r.TotalAmount : 0),
                        DoctorVcn = p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty,
                        PaymentMethod = r != null ? r.PaymentMethod : string.Empty
                    })
                    .ToList();
                dgPres.ItemsSource = data;
            }
            else
            {
                var data = context.Prescriptions
                    .Include(p => p.Receipts)
                    .Include(p => p.Patient)
                    .Include(p => p.PrescribingDoctorVcnNoNavigation)
                    .Where(p => p.PatientId == currentPatient.PatientId &&
                                p.Patient.ClientId.Equals(currentClient.ClientId) &&
                                (p.Id.ToString().Contains(searchText) ||
                                 p.PrescribingDoctorVcnNoNavigation.VcnNo.ToLower().Contains(searchText)))
                    .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                    {
                        IsSelected = false,
                        PrescriptionId = p.Id,
                        PrescriptionDate = p.Date,
                        PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                        ReceiptId = r != null ? r.Id : (int?)null,
                        ReceiptAmount = (decimal)(r != null ? r.TotalAmount : 0),
                        DoctorVcn = p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty,
                        PaymentMethod = r != null ? r.PaymentMethod : string.Empty
                    })
                    .ToList();
                dgPres.ItemsSource = data;
            }
        }

    }
}
