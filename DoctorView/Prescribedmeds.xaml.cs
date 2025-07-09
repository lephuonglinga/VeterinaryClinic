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
    /// Interaction logic for Prescribedmeds.xaml
    /// </summary>
    public partial class Prescribedmeds : Page
    {
        Prescription? prescription;
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public Prescribedmeds(Prescription? prescription)
        {
            if(prescription == null)
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
            DgPrescribedMeds.ItemsSource = context.Prescribedmeds.Where(pm => pm.PrescriptionId == prescription.Id)
                .ToList();
        }
    }
}
