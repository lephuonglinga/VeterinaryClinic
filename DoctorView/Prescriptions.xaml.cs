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
                var data = context.Prescriptions
                .Include(p => p.Receipts)
                .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                {
                    PrescriptionId = p.Id,
                    PrescriptionDate = p.Date,
                    PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                    ReceiptId = r != null ? r.Id : (int?)null,
                    ReceiptAmount = r != null ? r.TotalAmount : 0,
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
                    .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                    {
                        PrescriptionId = p.Id,
                        PrescriptionDate = p.Date,
                        PatientId = p.Patient != null ? p.Patient.PatientId : string.Empty,
                        ReceiptId = r != null ? r.Id : (int?)null,
                        ReceiptAmount = r != null ? r.TotalAmount : 0,
                        DoctorVcn = p.PrescribingDoctorVcnNoNavigation != null ? p.PrescribingDoctorVcnNoNavigation.VcnNo : string.Empty,
                        ReceiptDate = r != null ? r.Date : (DateOnly?)null
                    })
                    .Where(data => data.PatientId == currentPatient.PatientId &&
                                   data.DoctorVcn == (currentDoctor != null ? currentDoctor.VcnNo : string.Empty)) // Replaced null-coalescing operator with ternary operator
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
    }
}
