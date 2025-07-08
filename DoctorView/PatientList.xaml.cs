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
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.DoctorView
{
    /// <summary>
    /// Interaction logic for PatientList.xaml
    /// </summary>
    public partial class PatientList : Page
    {
        Client? currentClient;
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public PatientList(Client? client)
        {
            InitializeComponent();            
            currentClient = client;
            Page_Loaded();
        }

        private void Page_Loaded()
        {
            if (currentClient == null)
            {
                PatientDataGrid.ItemsSource = context.Patients.Include(p => p.Cases).Include(p => p.Breed).Where(p => p.Cases.Any(c => c.DoctorVcnNo == DoctorContext.CurrentDoctor.VcnNo)).ToList();
            }
            else {
                PatientDataGrid.ItemsSource = context.Patients.Include(p => p.Cases).Include(p => p.Breed).Where(p => p.ClientId == currentClient.ClientId && p.Cases.Any(c => c.DoctorVcnNo == DoctorContext.CurrentDoctor.VcnNo)).ToList();
            }               
        }

        private void ViewCases_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Patient? selectedPatient = button?.Tag as Patient;
            if (selectedPatient == null)
            {
                MessageBox.Show("No patient selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (selectedPatient.Cases.Count == 0)
            {
                MessageBox.Show("This patient has no cases.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
            doctorDetailsWindow.MainFrame.Navigate(new Cases(selectedPatient));
        }
    }
}
