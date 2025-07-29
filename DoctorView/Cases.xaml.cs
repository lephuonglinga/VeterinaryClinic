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
        bool isEdit = false;
        public Cases(Patient? patient)
        {
            currentPatient = patient;
            InitializeComponent();
            Page_Loaded();
            ComboBox_Load();
        }

        private void Page_Loaded()
        {
            Doctor? doctor = DoctorContext.CurrentDoctor;
            if (doctor == null)
            {
                MessageBox.Show("You are not logged in as a doctor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
            if (currentPatient != null)
            {
                CasesDataGrid.ItemsSource = context.Cases.Include(c => c.Diagnosis).Where(c => c.DoctorVcnNo == doctor.VcnNo && c.Patient.Id == currentPatient.Id).ToList();
            }
            else
            {
                CasesDataGrid.ItemsSource = context.Cases.Include(c => c.Diagnosis).Where(c => c.DoctorVcnNo == doctor.VcnNo).ToList();
            }
        }
        private void ComboBox_Load()
        {
            CbPatient.ItemsSource = context.Patients.ToList();
            CbPatient.DisplayMemberPath = "PatientId";
            CbPatient.SelectedValuePath = "PatientId";
            if(currentPatient != null)
            {
                CbPatient.SelectedValue = currentPatient.PatientId;
                CbPatient.IsEnabled = false;
            }            
            Diagnosis.ItemsSource = context.Diagnoses.ToList();
            Diagnosis.DisplayMemberPath = "Name";
            Diagnosis.SelectedValuePath = "Id";

            CaseType.ItemsSource = new List<string> { "Medical", "Surgical"};

            CaseType.SelectedIndex = 0;
            Diagnosis.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            BannerId.Visibility = Visibility.Collapsed;
            Title.Text = "Add New Case";
            CaseForm.Visibility = Visibility.Visible;
            CbPatient.SelectedValue = string.Empty;
            CbPatient.IsEnabled = true;
            isEdit = false;
            CaseId.Text = string.Empty;
            CaseType.SelectedIndex = 0;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            CaseForm.Visibility = Visibility.Collapsed;            
            Diagnosis.SelectedItem = null;
            Note.Text = "";
            CbPatient.SelectedItem = null;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CbPatient.SelectedItem == null || Diagnosis.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!isEdit)
            {
                Case newCase = new Case
                {
                    PatientId = CbPatient.SelectedValue.ToString(),
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Status = "New",
                    CaseType = CaseType.SelectedValue as string,
                    DiagnosisId = (int)Diagnosis.SelectedValue,
                    Notes = Note.Text,
                    DoctorVcnNo = DoctorContext.CurrentDoctor?.VcnNo
                };
                context.Cases.Add(newCase);
                context.SaveChanges();
            }
            else
            {
                if (!int.TryParse(CaseId.Text, out int caseId))
                {
                    MessageBox.Show("Invalid Case ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Case? @case = context.Cases.FirstOrDefault(c => c.Id == caseId);
                if (@case == null)
                {
                    MessageBox.Show("Please select a case to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                @case.PatientId = CbPatient.SelectedValue.ToString();
                @case.Date = DateOnly.FromDateTime(DateTime.Now);
                @case.Status = "Updated";
                @case.CaseType = CaseType.SelectedValue as string;
                @case.DiagnosisId = (int)Diagnosis.SelectedValue;
                @case.Notes = Note.Text;
                context.Cases.Update(@case);
                context.SaveChanges();
                MessageBox.Show("Case updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Page_Loaded();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Case @case = CasesDataGrid.SelectedItem as Case;
            if (@case == null)
            {
                MessageBox.Show("Please select a case to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BannerId.Visibility = Visibility.Visible;
            Title.Text = "Edit Case";
            CaseForm.Visibility = Visibility.Visible;
            CbPatient.SelectedValue = @case.PatientId;
            CbPatient.IsEnabled = false;
            isEdit = true;
            CaseId.Text = @case.Id.ToString();                     
            Diagnosis.SelectedValue = @case.DiagnosisId;
            CaseType.SelectedValue = @case.CaseType;
            Note.Text = @case.Notes;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null)
            {
                MessageBox.Show("Please select a case to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Case? @case = btn.Tag as Case;
            if (@case == null)
            {
                MessageBox.Show("Please select a case to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the case with ID: {@case.Id}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                context.Cases.Remove(@case);
                context.SaveChanges();
                MessageBox.Show("Case deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Page_Loaded();
            }
        }
    }
}
