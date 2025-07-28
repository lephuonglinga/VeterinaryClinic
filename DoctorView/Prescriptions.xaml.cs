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
using VeterinaryClinic.Models;

namespace VeterinaryClinic.DoctorView
{
    /// <summary>
    /// Interaction logic for Prescriptions.xaml
    /// </summary>
    public partial class Prescriptions : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        Patient? currentPatient;
        Doctor? currentDoctor = DoctorContext.CurrentDoctor;
        public Prescriptions(Patient? patient)
        {
            if (currentDoctor == null)
            {
                MessageBox.Show("No doctor is currently logged in.");
                return;
            }
            currentPatient = patient;
            InitializeComponent();
            Page_Loaded();
            ComboBox_Load();
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
                    .Where(p => p.PrescribingDoctorVcnNo == currentDoctor.VcnNo) 
                    .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new PrescriptionViewModel
                    {
                        IsSelected = false,
                        PrescriptionId = p.Id,
                        PrescriptionDate = p.Date,
                        PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                        ReceiptId = r != null ? r.Id : (int?)null,
                        ReceiptAmount = (decimal)(r != null ? r.TotalAmount : 0),
                        DoctorVcn = p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty,
                        ReceiptDate = r != null ? r.Date : (DateOnly?)null
                    })
                    .ToList();
                DgPrescription.ItemsSource = data;
            }
            else
            {
                var data = context.Prescriptions
                    .Include(p => p.Receipts)
                    .Include(p => p.Patient)
                    .Include(p => p.PrescribingDoctorVcnNoNavigation)
                    .Where(p => p.PatientId == currentPatient.PatientId &&
                       p.PrescribingDoctorVcnNo.Equals(currentDoctor.VcnNo))
                    .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new PrescriptionViewModel
                    {
                        IsSelected = false,
                        PrescriptionId = p.Id,
                        PrescriptionDate = p.Date,
                        PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                        ReceiptId = r != null ? r.Id : (int?)null,
                        ReceiptAmount = (decimal)(r != null ? r.TotalAmount : 0),
                        DoctorVcn = p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty,
                        ReceiptDate = r != null ? r.Date : (DateOnly?)null
                    })
                    .ToList();
                DgPrescription.ItemsSource = data;
            }
        }

        private void Presribedmeds_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if(button != null && button.Tag != null)
            {
                int PrescriptionId = (int)button.Tag;
                Prescription? prescription = context.Prescriptions.FirstOrDefault(p => p.Id == PrescriptionId);
                if (prescription == null)
                {
                    MessageBox.Show("Prescription not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
                doctorDetailsWindow.MainFrame.Navigate(new Prescribedmeds(prescription));
            }            
        }

        private void ComboBox_Load()
        {
            CbPatient.ItemsSource = context.Patients.ToList();
            CbPatient.DisplayMemberPath = "PatientId";
            CbPatient.SelectedValuePath = "PatientId";
            if (currentPatient != null)
            {
                CbPatient.SelectedValue = currentPatient.PatientId;
                CbPatient.IsEnabled = false;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CbPatient.SelectedItem == null && PatientForm.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Please select a patient to add a prescription.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CbPatient.SelectedItem == null && PatientForm.Visibility == Visibility.Collapsed)
            {
                PatientForm.Visibility = Visibility.Visible;
                return;
            }
            if (CbPatient.SelectedItem != null)
            {
                if (currentDoctor == null)
                {
                    MessageBox.Show("No doctor is currently logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Prescription prescription = new Prescription
                {
                    PatientId = CbPatient.SelectedValue.ToString(),
                    PrescribingDoctorVcnNo = currentDoctor.VcnNo,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                context.Prescriptions.Add(prescription);
                context.SaveChanges();
                MessageBox.Show("Add Successfully", "", MessageBoxButton.OK);
                Page_Loaded();
            }
        }

        bool isDelete = false;
        // Fix for the CS1061 error in the btnAction_Click method
        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            if (!isDelete)
            {
                SelectColumn.Visibility = Visibility.Visible;
                btnAction.Content = "Delete";
                btnAction.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                isDelete = true;
            }
            else
            {
                var itemsToDelete = new List<PrescriptionViewModel>();
                foreach (var item in DgPrescription.Items)
                {
                    if (item is PrescriptionViewModel viewModel && viewModel.IsSelected)
                    {
                        itemsToDelete.Add(viewModel);
                    }
                }

                if (itemsToDelete.Any())
                {
                    bool shouldProceed = true;

                    foreach (var viewModel in itemsToDelete)
                    {
                        int prescriptionId = viewModel.PrescriptionId;

                        // Load the prescription with all related entities
                        var prescription = context.Prescriptions
                            .Include(p => p.Prescribedmeds)
                            .Include(p => p.Receipts)
                            .FirstOrDefault(p => p.Id == prescriptionId);

                        if (prescription != null)
                        {
                            // Check and delete prescribedmeds if any exist
                            if (prescription.Prescribedmeds != null && prescription.Prescribedmeds.Any())
                            {
                                var result = MessageBox.Show(
                                    $"Prescription {prescriptionId} has {prescription.Prescribedmeds.Count} prescribed medications. Are you sure you want to delete them?",
                                    "Confirm Deletion",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                );

                                if (result == MessageBoxResult.Yes)
                                {
                                    context.Prescribedmeds.RemoveRange(prescription.Prescribedmeds);
                                }
                                else
                                {
                                    shouldProceed = false;
                                    continue; // Skip this prescription
                                }
                            }

                            // Check and delete receipts if any exist
                            if (prescription.Receipts != null && prescription.Receipts.Any())
                            {
                                var result = MessageBox.Show(
                                    $"Prescription {prescriptionId} has {prescription.Receipts.Count} receipts. Are you sure you want to delete them?",
                                    "Confirm Deletion",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning
                                );

                                if (result == MessageBoxResult.Yes)
                                {
                                    context.Receipts.RemoveRange(prescription.Receipts);
                                }
                                else
                                {
                                    shouldProceed = false;
                                    continue; // Skip this prescription
                                }
                            }

                            // Finally delete the prescription
                            context.Prescriptions.Remove(prescription);
                        }
                    }

                    // Save all changes at once, outside the loop
                    if (shouldProceed)
                    {
                        try
                        {
                            context.SaveChanges();
                            MessageBox.Show("Selected prescriptions deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting prescriptions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    // Reset the button state
                    SelectColumn.Visibility = Visibility.Collapsed;
                    btnAction.Content = "Select to Delete";
                    btnAction.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
                    isDelete = false;

                    Page_Loaded();
                }
                else
                {
                    MessageBox.Show("No items selected for deletion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                    .Where(p => p.PrescribingDoctorVcnNoNavigation.VcnNo == currentDoctor.VcnNo &&
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
                DgPrescription.ItemsSource = data;
            }
            else
            {
                var data = context.Prescriptions
                    .Include(p => p.Receipts)
                    .Include(p => p.Patient)
                    .Include(p => p.PrescribingDoctorVcnNoNavigation)
                    .Where(p => p.PatientId == currentPatient.PatientId &&
                                p.PrescribingDoctorVcnNoNavigation.VcnNo == currentDoctor.VcnNo &&
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
                DgPrescription.ItemsSource = data;
            }
        }
    }

    public class PrescriptionViewModel
    {
        public bool IsSelected { get; set; }
        public int PrescriptionId { get; set; }
        public DateOnly? PrescriptionDate { get; set; }
        public string PatientId { get; set; } = string.Empty;
        public int? ReceiptId { get; set; }
        public decimal ReceiptAmount { get; set; }
        public string DoctorVcn { get; set; } = string.Empty;
        public DateOnly? ReceiptDate { get; set; }
    }
}
