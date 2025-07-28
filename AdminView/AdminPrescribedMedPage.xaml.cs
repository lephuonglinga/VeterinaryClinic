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

namespace VeterinaryClinic.AdminView
{
    /// <summary>
    /// Interaction logic for AdminPrescribedMedPage.xaml
    /// </summary>
    public partial class AdminPrescribedMedPage : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        User? currentUser;
        Prescription? prescription;
        public AdminPrescribedMedPage(Prescription? prescription)
        {
            currentUser = AdminContext.CurrentAdmin;
            if (currentUser == null)
            {
                MessageBox.Show("No account is currently logged in");
                return;
            }
            this.prescription = prescription;
            InitializeComponent();
            Page_Loaded();
        }

        private void Page_Loaded()
        {            
            if(prescription != null)
            {
                DgPresMed.ItemsSource = context.Prescribedmeds.Where(pm => pm.PrescriptionId == prescription.Id).Include(pm => pm.Medication)
                .ToList();
            }
            else
            {
                DgPresMed.ItemsSource = context.Prescribedmeds.Include(pm => pm.Medication)
                .ToList();
            }
            

        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            if (prescription != null)
            {
                DgPresMed.ItemsSource = context.Prescribedmeds.Where(pm => (pm.Frequency.ToLower().Contains(searchText) || pm.Route.ToLower().Contains(searchText) || pm.Medication.GenericName.ToLower().Contains(searchText) || pm.Medication.TradeName.ToLower().Contains(searchText)) && pm.PrescriptionId == prescription.Id).Include(pm => pm.Medication)
                .ToList();
            }
            else
            {
                DgPresMed.ItemsSource = context.Prescribedmeds.Include(pm => pm.Medication).Where(pm => (pm.Frequency.ToLower().Contains(searchText) || pm.Route.ToLower().Contains(searchText) || pm.Medication.GenericName.ToLower().Contains(searchText) || pm.Medication.TradeName.ToLower().Contains(searchText)))
                .ToList();
            }
        }
    }
}
