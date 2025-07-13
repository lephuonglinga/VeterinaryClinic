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
            else
            {
                MessageBox.Show("No patient selected. Please select a patient to view their cases.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                Diagnosis.ItemsSource = context.Diagnoses.ToList();
            Diagnosis.DisplayMemberPath = "Name";
            Diagnosis.SelectedValuePath = "Id";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            isEdit = false;
            Title.Text = "Add New Case";
            CaseForm.Visibility = Visibility.Visible;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            CaseForm.Visibility = Visibility.Collapsed;
            Date.Text = "";
            Status.Text = "";
            Diagnosis.SelectedItem = null;
            Note.Text = "";
            CbPatient.SelectedItem = null;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CbPatient.SelectedItem == null || Diagnosis.SelectedItem == null || string.IsNullOrWhiteSpace(Date.Text) || string.IsNullOrWhiteSpace(Status.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!isEdit)
            {
                Case newCase = new Case
                {
                    PatientId = CbPatient.SelectedValue.ToString(),
                    Date = DateOnly.Parse(Date.Text),
                    Status = Status.Text,
                    CaseType = CaseType.Text,
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

                if (DateOnly.TryParse(Date.Text, out DateOnly caseDate))
                {
                    @case.Date = caseDate;
                }
                else
                {
                    MessageBox.Show("Invalid date format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                @case.Status = "Updated";
                @case.CaseType = CaseType.Text;
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
        }
    }
}
