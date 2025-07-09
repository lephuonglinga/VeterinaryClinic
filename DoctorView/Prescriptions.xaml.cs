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
            InitializeComponent();
            Page_Loaded();
            currentPatient = patient;
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
    }
}
