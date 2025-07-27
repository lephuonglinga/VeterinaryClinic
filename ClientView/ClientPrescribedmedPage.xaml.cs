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
using VeterinaryClinic.DoctorView;
using VeterinaryClinic.Models;
using Microsoft.EntityFrameworkCore;

namespace VeterinaryClinic.ClientView
{
    /// <summary>
    /// Interaction logic for ClientPrescribedmedPage.xaml
    /// </summary>    
    public partial class ClientPrescribedmedPage : Page
    {
        Prescription? prescription;
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public ClientPrescribedmedPage(Prescription? prescription)
        {
            if (prescription == null)
            {
                MessageBox.Show("No prescriptions available.");
                return;
            }
            this.prescription = prescription;
            InitializeComponent();
            Page_Loaded();
        }

        private void Page_Loaded()
        {
            if (prescription == null)
            {
                MessageBox.Show("Prescription is null. Cannot load prescribed medications.");
                return;
            }

            var data = context.Prescribedmeds.Where(pm => pm.PrescriptionId == prescription.Id)
                .ToList();

            DgPresMed.ItemsSource = context.Prescribedmeds.Where(pm => pm.PrescriptionId == prescription.Id).Include(pm => pm.Medication)
                .ToList();
            
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            if (prescription == null)
            {
                MessageBox.Show("Prescription is null. Cannot filter prescribed medications.");
                return;
            }
            DgPresMed.ItemsSource = context.Prescribedmeds.Include(pm => pm.Medication)
                .Where(pm => pm.PrescriptionId == prescription.Id && 
                             (pm.Medication.TradeName.ToLower().Contains(searchText) || 
                              pm.Medication.Category.ToLower().Contains(searchText) ||
                              pm.Route.ToLower().Contains(searchText) || 
                              pm.Frequency.ToLower().Contains(searchText)))
                .Include(pm => pm.Medication)
                .ToList();
        }
    }
}
