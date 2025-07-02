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
using VeterinaryClinic.Service;

namespace VeterinaryClinic
{
    /// <summary>
    /// Interaction logic for MyPets.xaml
    /// </summary>
    public partial class MyPets : Page
    {
        private readonly Client currentClient;
        private readonly PatientService patientService;
        private readonly VeterinaryClinicContext context = new VeterinaryClinicContext();

        public MyPets()
        {
            Client? client = ClientContext.CurrentClient;
            InitializeComponent();
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");
            }
            currentClient = client;
            patientService = new PatientService();
            LoadPatientByClient();
        }

        private void LoadPatientByClient()
        {
            try
            {
                var patients = context.Patients
                .Include(p => p.Breed)
                .Include(p => p.Species)
                .Where(p => p.ClientId == currentClient.ClientId)
                .ToList();                

                dgPatients.ItemsSource = patients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\nChi tiết: {ex.InnerException?.Message}");

                var fullError = new StringBuilder();
                fullError.AppendLine("Message: " + ex.Message);
                fullError.AppendLine("StackTrace: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    fullError.AppendLine("InnerException: " + ex.InnerException.Message);
                    fullError.AppendLine("Inner StackTrace: " + ex.InnerException.StackTrace);
                }
                MessageBox.Show(fullError.ToString());
            }

        }
        
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedPatient = button?.Tag as Patient;

            if (selectedPatient != null)
            {                
                NavigationService?.Navigate(new VeterinaryClinic.CaseDetailsPage(selectedPatient));
            }
        }

    }

}
