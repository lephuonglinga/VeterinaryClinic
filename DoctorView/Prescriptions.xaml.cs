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
            if(currentDoctor == null)
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
            if(currentPatient == null)
            {
                DgPrescription.ItemsSource = context.Prescriptions
                    .Where(p => p.PrescribingDoctorVcnNo == currentDoctor.VcnNo)
                    .ToList();
            }
            else
            {
                DgPrescription.ItemsSource = context.Prescriptions
                    .Where(p => p.PatientId == currentPatient.PatientId &&
                                p.PrescribingDoctorVcnNo == currentDoctor.VcnNo)
                    .ToList();
            }
        }

        private void Presribedmeds_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if(button != null && button.Tag != null)
            {
                Prescription prescription = button.Tag as Prescription;
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
            if(CbPatient.SelectedItem == null && PatientForm.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Please select a patient to add a prescription.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CbPatient.SelectedItem == null && PatientForm.Visibility == Visibility.Collapsed)
            {
                PatientForm.Visibility = Visibility.Visible;
                return;
            }
            if(CbPatient.SelectedItem != null)
            {
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
