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
using Microsoft.EntityFrameworkCore;

namespace VeterinaryClinic.DoctorView
{
    /// <summary>
    /// Interaction logic for Cases.xaml
    /// </summary>
    public partial class Cases : Page
    {
        Patient? currentPatient;
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public Cases(Patient? patient)
        {            
            currentPatient = patient;
            InitializeComponent();
            Page_Loaded();
        }

        private void Page_Loaded()
        {            
            Doctor? doctor = DoctorContext.CurrentDoctor;
            if (doctor == null)
            {
                MessageBox.Show("You are not logged in as a doctor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CasesDataGrid.Items.Clear();
            if(currentPatient != null)
            {
                CasesDataGrid.ItemsSource = context.Cases.Include(c => c.Diagnosis).Where(c => c.DoctorVcnNo == doctor.VcnNo && c.Patient.Id == currentPatient.Id).ToList();
            }
            else
            {
                CasesDataGrid.ItemsSource = context.Cases.Include(c => c.Diagnosis).Where(c => c.DoctorVcnNo == doctor.VcnNo).ToList();
            }                
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
