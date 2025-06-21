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

namespace VeterinaryClinic
{
    /// <summary>
    /// Interaction logic for CaseDetailsPage.xaml
    /// </summary>
    public partial class CaseDetailsPage : Page
    {
        private readonly VeterinaryClinicContext context = new VeterinaryClinicContext();

        public CaseDetailsPage(Patient patient)
        {
            InitializeComponent();

            if (patient?.PatientId != null)
            {
                List<Case> cases = context.Cases
                    .Where(c => c.PatientId == patient.PatientId)
                    .ToList();

                dgCases.ItemsSource = cases;
            }
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void SidebarMenu_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Sidebar loaded");
        }
    }
}
