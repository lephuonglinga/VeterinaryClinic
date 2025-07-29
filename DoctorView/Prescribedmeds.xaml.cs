using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
using VeterinaryClinic.DTOs;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.DoctorView
{
    /// <summary>
    /// Interaction logic for Prescribedmeds.xaml
    /// </summary>
    public partial class Prescribedmeds : Page
    {
        private readonly CaseBackgroundDataService _caseService;
        private readonly PharmacyInventoryDataService _pharmacyService;
        private readonly GeminiMedicationSuggestionService _geminiService;

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

            _caseService = new CaseBackgroundDataService(new VeterinaryClinicContext());
            _pharmacyService = new PharmacyInventoryDataService(new VeterinaryClinicContext());
            _geminiService = new GeminiMedicationSuggestionService();

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

            DgPrescribedMeds.ItemsSource = context.Prescribedmeds.Where(pm => pm.PrescriptionId == prescription.Id).Include(pm => pm.Medication)
                .ToList();

            Receipt? receipt = context.Receipts.FirstOrDefault(r => r.PrescriptionId == prescription.Id);
            if (receipt == null)
            {
                MessageBox.Show("No receipt found for the given prescription.");
                return;
            }

            decimal totalAmount = 0;
            foreach (var item in data)
            {
                if (item.Medication?.SalePricePerUnit != null && item.Quantity != null)
                {
                    totalAmount += (decimal)(item.Medication.SalePricePerUnit * item.Quantity);
                }
                else {                    
                    MessageBox.Show("Medication sale price or quantity is null for one or more prescribed medications.");
                    return;
                }
            }
            receipt.TotalAmount = (decimal) totalAmount;

            context.Update(receipt);
            context.SaveChanges();
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
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Reset();

            AISuggestion.Children.Clear();
            AISuggestion.Visibility = Visibility.Visible;

            MedId.ItemsSource = _pharmacyService.GetAvailablePharmacyInventoriesExcluding(
                prescription.PatientId ?? "");

            MedId.DisplayMemberPath = "TradeName";
            MedId.SelectedValuePath = "ItemId";
            if (MedId.Items.Count > 0)
            {
                MedId.SelectedIndex = 0;
            }

            ShowAISuggestions();

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
            
            Quantity.Text = selectedMed.Quantity.ToString();
            Freq.Text = selectedMed.Frequency;
            Route.Text = selectedMed.Route;                    
            Open_Form(true);            
        }

        private void Open_Form(bool edit)
        {
            PresMedForm.Visibility = Visibility.Visible;
            MedId.ItemsSource = context.PharmacyInventories.ToList();
            MedId.DisplayMemberPath = "TradeName";
            MedId.SelectedValuePath = "ItemId";
            MedId.SelectedIndex = 0;
            isEdit = true;
            PresId.Text = prescription != null ? prescription.Id.ToString() : string.Empty;
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
            if (MedId.SelectedValue == null)
            {
                MessageBox.Show("Please Choose Medication", "Error");
                return;
            }
            if (Quantity == null || string.IsNullOrEmpty(Quantity.Text) || string.IsNullOrEmpty(Route.Text) || string.IsNullOrEmpty(Freq.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Error");
                return;
            }

            if (!isEdit)
            {
                Prescribedmed newPrescribedmed = new Prescribedmed();
                newPrescribedmed.PrescriptionId = prescription.Id;
                newPrescribedmed.MedicationId = (int)MedId.SelectedValue;                
                if (int.TryParse(Quantity.Text, out int quantity))
                {
                    newPrescribedmed.Quantity = quantity;
                }
                else
                {
                    MessageBox.Show("Invalid quantity value.", "Error");
                    return;
                }

                newPrescribedmed.Frequency = Freq.Text;
                newPrescribedmed.Route = Route.Text;
                newPrescribedmed.Date = DateOnly.FromDateTime(DateTime.Now);
                bool IsNewAdd = false;
                Receipt? receipt = context.Receipts.FirstOrDefault(r => r.PrescriptionId == prescription.Id);
                if(receipt is null)
                {
                    receipt = new Receipt();
                    IsNewAdd = true;
                }
                receipt.PrescriptionId = prescription.Id;
                receipt.Date = DateOnly.FromDateTime(DateTime.Now);
                receipt.TotalAmount = 0;
                if (IsNewAdd)
                {
                    context.Receipts.Add(receipt);
                }
                else
                {
                    context.Receipts.Update(receipt);
                }

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
                
                if (int.TryParse(Quantity.Text, out int quantity))
                {
                    selectedMed.Quantity = quantity;
                }
                else
                {
                    MessageBox.Show("Invalid quantity value.", "Error");
                    return;
                }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button == null)
            {
                MessageBox.Show("Prescribemed is null. Cannot perform action.");
                return;
            }
            Prescribedmed? selectedMed = button.Tag as Prescribedmed;
            if (selectedMed == null)
            {
                MessageBox.Show("Please select a medication to delete.");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this prescribed medication?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                context.Prescribedmeds.Remove(selectedMed);
                context.SaveChanges();
                MessageBox.Show("Prescribed medication deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Page_Loaded();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            if(string.IsNullOrEmpty(searchText))
            {
                DgPrescribedMeds.ItemsSource = context.Prescribedmeds.Where(pm => pm.PrescriptionId == prescription.Id).Include(pm => pm.Medication)
                    .ToList();
                return;
            }else
            {
                DgPrescribedMeds.ItemsSource = context.Prescribedmeds.Where(pm => pm.PrescriptionId == prescription.Id && 
                    (pm.Medication.TradeName.ToLower().Contains(searchText) || pm.Medication.GenericName.ToLower().Contains(searchText)))
                    .Include(pm => pm.Medication)
                    .ToList();
            }            
        }

        // Call this method when 'Add' button is clicked
        private void ShowAISuggestions()
        {
            if (prescription == null)
                return;

            // Clear previous suggestions UI
            AISuggestion.Children.Clear();

            // 1. Load background cases for the patient
            var patientCases = _caseService.GetCasesByPatientId(prescription.PatientId ?? "");

            // 2. Load candidate medications excluding already prescribed meds
            var candidateMeds = _pharmacyService.GetAvailablePharmacyInventoriesExcluding(prescription.PatientId ?? "").ToList();

            // 3. Build prompt string for Gemini
            string prompt = _geminiService.BuildPrompt(prescription.PatientId ?? "", patientCases, candidateMeds);

            // 4. Get medication suggestions from Gemini
            var suggestions = _geminiService.GetMedicationSuggestions(prompt, enableLogging: true);

            if (suggestions.Count == 0)
            {
                TextBlock noSuggestionsText = new TextBlock() { Text = "No AI medication suggestions available.", Margin = new Thickness(5) };
                AISuggestion.Children.Add(noSuggestionsText);
                return;
            }

            // 5. Render each suggestion as a UI panel inside AISuggestion StackPanel
            foreach (var suggestion in suggestions)
            {
                StackPanel panel = new StackPanel()
                {
                    Margin = new Thickness(10),
                    Background = Brushes.WhiteSmoke,
                    Width = 350
                };

                void AddLabelAndText(string label, string text)
                {
                    panel.Children.Add(new TextBlock() { Text = label, FontWeight = FontWeights.Bold });
                    panel.Children.Add(new TextBlock() { Text = text, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 8) });
                }

                AddLabelAndText("Medication ID:", suggestion.MedicationId.ToString());
                AddLabelAndText("Reason:", suggestion.Reason);
                AddLabelAndText("Description:", suggestion.Description);
                AddLabelAndText("Quantity:", suggestion.Quantity?.ToString() ?? "N/A");
                AddLabelAndText("Frequency:", suggestion.Frequency ?? "N/A");
                AddLabelAndText("Route:", suggestion.Route ?? "N/A");

                Button addButton = new Button()
                {
                    Content = "Add This Medication",
                    Width = 140,
                    Height = 30,
                    Margin = new Thickness(0, 10, 0, 0),
                    Tag = suggestion // Store suggestion object for use in click handler
                };
                addButton.Click += SuggestedMedAdd_Click;               

                panel.Children.Add(addButton);

                AISuggestion.Children.Add(panel);
            }

            AISuggestion.Visibility = Visibility.Visible;
        }

        private void SuggestedMedAdd_Click(object sender, RoutedEventArgs e)
        {            
            if (sender is Button btn && btn.Tag is GeminiMedicationSuggestionDTO suggestion)
            {
                var meds = MedId.ItemsSource as IEnumerable<PharmacyInventory>;
                if (meds == null || meds.Count() == 0)  return;

                var medToSelect = meds.FirstOrDefault(m => m.ItemId == suggestion.MedicationId);
                if (medToSelect == null) {
                    MessageBox.Show("NOT FOUND");
                    return; }

                MedId.SelectedValue = medToSelect.ItemId;
                Quantity.Text = suggestion.Quantity?.ToString() ?? "N/A";
                Freq.Text = suggestion.Frequency ?? "N/A";
                Route.Text = suggestion.Route ?? "N/A";

                if (PresMedForm.Visibility != Visibility.Visible)
                    PresMedForm.Visibility = Visibility.Visible;

                AISuggestion.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("NO RESULT");
            }
        }
    }
}
