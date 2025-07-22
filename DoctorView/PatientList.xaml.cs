using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repository;

namespace VeterinaryClinic.DoctorView
{
    /// <summary>
    /// Interaction logic for PatientList.xaml
    /// </summary>
    public partial class PatientList : Page
    {
        Client? currentClient;
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        bool isEdit = false;
        public PatientList(Client? client)
        {
            InitializeComponent();            
            currentClient = client;
            Page_Loaded();
            ComboBox_Load();
        }

        private void Page_Loaded()
        {            
            if (currentClient == null)
            {
                PatientDataGrid.ItemsSource = context.Patients.Include(p => p.Cases).Include(p => p.Breed).Where(p => p.Cases.Any(c => c.DoctorVcnNo == DoctorContext.CurrentDoctor.VcnNo)).ToList();
            }
            else {
                PatientDataGrid.ItemsSource = context.Patients.Include(p => p.Cases).Include(p => p.Breed).Where(p => p.ClientId == currentClient.ClientId && p.Cases.Any(c => c.DoctorVcnNo == DoctorContext.CurrentDoctor.VcnNo)).ToList();
            }               
        }

        private void ComboBox_Load()
        {
            CbBreed.ItemsSource = context.Breeds.ToList();
            CbBreed.DisplayMemberPath = "Name";
            CbBreed.SelectedValuePath = "Id";            

            CbSpecies.ItemsSource = context.Species.ToList();
            CbSpecies.DisplayMemberPath = "Name";
            CbSpecies.SelectedValuePath = "Id";

            CbClient.ItemsSource = context.Clients.Include(c => c.UsernameNavigation).ToList();
            if (currentClient != null)
            {
                CbClient.SelectedValue = currentClient.ClientId;
                CbClient.IsEnabled = false; // Disable selection if a specific client is provided
            }            
            CbClient.DisplayMemberPath = "DisplayName";
            CbClient.SelectedValuePath = "ClientId";
        }

        private void ViewCases_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            Patient? selectedPatient = button?.Tag as Patient;
            if (selectedPatient == null)
            {
                MessageBox.Show("No patient selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (selectedPatient.Cases.Count == 0)
            {
                MessageBox.Show("This patient has no cases.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
            doctorDetailsWindow.MainFrame.Navigate(new Cases(selectedPatient));
        }  
        
        private void Reset()
        {
            ComboBox_Load();
            Sex.Text = "";
            AgeGroup.Text = "";
        }        

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            isEdit = false;
            PatientForm.Visibility = Visibility.Visible;

            //format the form for editing                        
            PatientId.Visibility = Visibility.Collapsed;
            PatientId.Text = "";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            PatientForm.Visibility = Visibility.Collapsed;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                Patient? patient = context.Patients.FirstOrDefault(p => p.PatientId == PatientId.Text);
                if (patient != null)
                {
                    patient.BreedId = (int)CbBreed.SelectedValue;
                    patient.SpeciesId = (int)CbSpecies.SelectedValue;
                    patient.Sex = Sex.Text;
                    patient.AgeGroup = "Young";
                    context.Patients.Update(patient);
                    context.SaveChanges();
                    MessageBox.Show("Edit Successfully!", "", MessageBoxButton.OK);
                    Page_Loaded();
                }
                else
                {
                    MessageBox.Show("Patient not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                    return;
            }
            else
            {
                Patient? patient = new Patient();
                if (CbBreed.SelectedItem == null || CbSpecies.SelectedItem == null || CbClient.SelectedItem == null)
                {
                    MessageBox.Show("Please select a breed, species, and client.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bool isAddNewCase = MessageBox.Show("Do you want to add new Case for patient ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
                if (!isAddNewCase)
                {
                    MessageBox.Show("Cannot add new Patient", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //add patient
                patient.BreedId = (int)CbBreed.SelectedValue;
                patient.SpeciesId = (int)CbSpecies.SelectedValue;
                patient.ClientId = CbClient.SelectedValue.ToString();
                patient.Sex = Sex.Text;
                patient.AgeGroup = "Young";
                string patientId;

                //generrate unique patientId
                do
                {
                    patientId = "PID-" + IdGenerator.GenerateUserId();
                } while (context.Patients.Any(p => p.PatientId == patientId));
                patient.PatientId = patientId;
                context.Patients.Add(patient);

                //add case for patient
                Case newCase = new Case();
                newCase.PatientId = patient.PatientId;
                newCase.Status = "New";
                newCase.Date = DateOnly.FromDateTime(DateTime.Now);
                newCase.DoctorVcnNo = DoctorContext.CurrentDoctor.VcnNo;
                context.Cases.Add(newCase);
                context.SaveChanges();
                MessageBox.Show("Patient added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Reset();
                PatientForm.Visibility = Visibility.Collapsed;
                Page_Loaded();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Patient? selectedPatient = PatientDataGrid.SelectedItem as Patient;
            if (selectedPatient == null)
            {
                MessageBox.Show("Please select a patient to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PatientForm.Visibility = Visibility.Visible;
            Title.Text = "Edit Patient";
            isEdit = true;
            CbClient.SelectedValue = selectedPatient.ClientId;
            CbClient.IsEnabled = false;

            //format the form for editing                                    
            PatientId.Visibility = Visibility.Visible;
            PatientId.Text = "Patient ID: " + selectedPatient.PatientId;
        }

        private void ViewPres_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            Patient? selectedPatient = button?.Tag as Patient;
            if (selectedPatient == null)
            {
                MessageBox.Show("No patient selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (selectedPatient.Cases.Count == 0)
            {
                MessageBox.Show("This patient has no cases.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
            doctorDetailsWindow.MainFrame.Navigate(new Prescriptions(selectedPatient));
        }
    }
}
