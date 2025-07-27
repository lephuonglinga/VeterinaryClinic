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
        private Patient? currentPatient;
        public CaseDetailsPage(Patient patient)
        {
            InitializeComponent();

            Page_Loaded(patient);
            currentPatient = patient;
            Combobox_Loaded();
        }

        private void Page_Loaded(Patient? patient)
        {
            if (patient?.PatientId != null)
            {
                List<Case> cases = context.Cases
                    .Where(c => c.PatientId == patient.PatientId)
                    .ToList();

                dgCases.ItemsSource = cases;
            }
        }

        private void Combobox_Loaded()
        {
            TypeFilterComboBox.Items.Clear();
            TypeFilterComboBox.ItemsSource = new List<string>
            {
                "All", "Medical", "Surgical"
            };

            StatusFilterComboBox.Items.Clear();
            StatusFilterComboBox.ItemsSource = new List<string>
            {
               "All", "New", "Updated"
            };
            TypeFilterComboBox.SelectedIndex = 0; // Default to "All"
            StatusFilterComboBox.SelectedIndex = 0; // Default to "All"
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void TypeFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page_Loaded(currentPatient);
            var list = dgCases.ItemsSource as List<Case>;
            if (list != null)
            {
                if (TypeFilterComboBox.SelectedValue is string selectedItem)
                {
                    string selectedType = selectedItem;
                    if (selectedType == "All")
                    {
                        dgCases.ItemsSource = list;
                    }
                    else
                    {
                        dgCases.ItemsSource = list.Where(c => c.CaseType == selectedType).ToList();
                    }
                }
            }
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page_Loaded(currentPatient);
            var list = dgCases.ItemsSource as List<Case>;
            if (list != null)
            {
                if (StatusFilterComboBox.SelectedValue is string selectedItem)
                {
                    string selectedStatus = selectedItem;
                    if (selectedStatus == "All")
                    {
                        dgCases.ItemsSource = list;
                    }
                    else
                    {
                        dgCases.ItemsSource = list.Where(c => c.Status == selectedStatus).ToList();
                    }
                }
            }
        }
    }
}
