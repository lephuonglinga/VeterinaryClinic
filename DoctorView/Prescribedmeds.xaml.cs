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
        bool isEdit = false;
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

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PresMedForm.Visibility = Visibility.Collapsed;
            Reset();
        }

        private void Reset()
        {
            MedId.Text = "";
            MedId.IsEnabled = true;
            Quantity.Text = "";
            Freq.Text = "";
            Route.Text = "";
            Date.Text = "";            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            Open_Form(false);            
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Prescribedmed? selectedMed = DgPrescribedMeds.SelectedItem as Prescribedmed;
            if (selectedMed == null)
            {
                MessageBox.Show("Please select a medication to edit.");
                return;
            }
            MedId.SelectedItem = selectedMed.Medication;
            MedId.Text = selectedMed.Medication?.TradeName ?? string.Empty;
            MedId.IsEnabled = false;
            Quantity.Text = selectedMed.Quantity.ToString();
            Freq.Text = selectedMed.Frequency;
            Route.Text = selectedMed.Route;
            Date.Text = selectedMed.Date?.ToString("dd-MM-YYY") ?? DateTime.Now.ToString("dd-MM-yyyy");            
            Open_Form(true);            
        }

        private void Open_Form(bool edit)
        {
            PresMedForm.Visibility = Visibility.Visible;
            MedId.ItemsSource = context.PharmacyInventories.ToList();
            MedId.DisplayMemberPath = "TradeName";
            MedId.SelectedValuePath = "ItemId";
            isEdit = true;
            PresId.Text = prescription.Id.ToString();
            isEdit = edit;
            if (edit)
            {
                Title.Text = "Edit Prescribed Meds";
            }
            else
            {
                Title.Text = "Add Prescribed Meds";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {            
            if (!isEdit)
            {
                Prescribedmed newPrescribedmed = new Prescribedmed();                
                newPrescribedmed.PrescriptionId = prescription.Id;
                newPrescribedmed.MedicationId = (int)MedId.SelectedValue;
                newPrescribedmed.Quantity = int.Parse(Quantity.Text);
                newPrescribedmed.Frequency = Freq.Text;
                newPrescribedmed.Route = Route.Text;
                newPrescribedmed.Date = DateOnly.FromDateTime(DateTime.Now);
                context.Prescribedmeds.Add(newPrescribedmed);
                context.SaveChanges();
            }
            else
            {
                Prescribedmed? selectedMed = DgPrescribedMeds.SelectedItem as Prescribedmed;
                if (selectedMed == null)
                {
                    MessageBox.Show("Please select a medication to edit.");
                    return;
                }
                selectedMed.MedicationId = (int)MedId.SelectedValue;
                selectedMed.Quantity = int.Parse(Quantity.Text);
                selectedMed.Frequency = Freq.Text;
                selectedMed.Route = Route.Text;
                selectedMed.Date = DateOnly.FromDateTime(DateTime.Now);
                context.Prescribedmeds.Update(selectedMed);
                context.SaveChanges();
                MessageBox.Show("Prescribed medication updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            PresMedForm.Visibility = Visibility.Collapsed;
            Page_Loaded();
        }
        
    }
}
