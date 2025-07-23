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
    /// Interaction logic for Prescription.xaml
    /// </summary>
    public partial class Prescription : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public Prescription()
        {
            InitializeComponent();
            Page_Loaded();
        }

        private void Page_Loaded()
        {
            var data = context.Prescriptions
                .Include(p => p.Receipts)
                .SelectMany(p => p.Receipts.DefaultIfEmpty(), (p, r) => new
                {
                    PrescriptionId = p.Id,
                    PrescriptionDate = p.Date,
                    PatientId = p.Patient.PatientId,
                    ReceiptId = r != null ? r.Id : (int?)null,
                    ReceiptAmount = r != null ? r.TotalAmount : 0,
                    DoctorVcn = p.PrescribingDoctorVcnNoNavigation.VcnNo,
                    ReceiptDate = r != null ? r.Date : (DateOnly?)null
                })
                .ToList();
            PrescriptionDataGrid.ItemsSource = data;
        }
    }
}
