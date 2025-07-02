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
            if (client == null)
            {
                MessageBox.Show("No client selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            currentClient = client;
            Page_Loaded();
        }

        private void Page_Loaded()
        {
            if (ClientContext.CurrentClient == null)
            {
                MessageBox.Show("No client selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PatientDataGrid.ItemsSource = context.Patients.Include(p => p.Cases).Include(p => p.Breed).Where(p => p.ClientId == currentClient.ClientId &&p.Cases.Any(c => c.DoctorVcnNo == DoctorContext.CurrentDoctor.VcnNo)).ToList();

        }
    }
}
