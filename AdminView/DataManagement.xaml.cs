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
    /// Interaction logic for DataManagement.xaml
    /// </summary>
    public partial class DataManagement : Page
    {
        private VeterinaryClinicContext _context;
        public DataManagement()
        {
            InitializeComponent();
            _context = new VeterinaryClinicContext();
            LoadData();
            DiagnosisGrid.SelectionChanged += DiagnosisGrid_SelectionChanged;
            BreedGrid.SelectionChanged += BreedGrid_SelectionChanged;
            SpeciesGrid.SelectionChanged += SpeciesGrid_SelectionChanged;
        }
        private void LoadData()
        {
            DiagnosisGrid.ItemsSource = _context.Diagnoses.ToList();
            BreedGrid.ItemsSource = _context.Breeds.ToList();
            SpeciesGrid.ItemsSource = _context.Species.ToList();

            SpeciesUnitTextBox.ItemsSource = new List<string>
            {
                "Small Animal", "Large Animal"
            };
            SpeciesUnitTextBox.SelectedIndex = 0;            
        }

        private void DiagnosisGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DiagnosisGrid.SelectedItem is Diagnosis selected)
            {
                DiagnosisNameTextBox.Text = selected.Name;
            }
        }

        // Khi chọn một dòng trong BreedGrid
        private void BreedGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BreedGrid.SelectedItem is Breed selected)
            {
                BreedNameTextBox.Text = selected.Name;
            }
        }

        // Khi chọn một dòng trong SpeciesGrid
        private void SpeciesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpeciesGrid.SelectedItem is Species selected)
            {
                SpeciesNameTextBox.Text = selected.Name;
                SpeciesUnitTextBox.SelectedValue = selected.Unit;
            }
        }


        private void AddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            var name = DiagnosisNameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var newDiag = new Diagnosis { Name = name };
                _context.Diagnoses.Add(newDiag);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            if (DiagnosisGrid.SelectedItem is Diagnosis selected)
            {
                selected.Name = DiagnosisNameTextBox.Text.Trim();
                _context.Diagnoses.Update(selected);
                _context.SaveChanges();
                LoadData();
            }
        }

        // Breed Add/Edit
        private void AddBreed_Click(object sender, RoutedEventArgs e)
        {
            var name = BreedNameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var newBreed = new Breed { Name = name };
                _context.Breeds.Add(newBreed);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditBreed_Click(object sender, RoutedEventArgs e)
        {
            if (BreedGrid.SelectedItem is Breed selected)
            {
                selected.Name = BreedNameTextBox.Text.Trim();
                _context.Breeds.Update(selected);
                _context.SaveChanges();
                LoadData();
            }
        }

        // Species Add/Edit
        private void AddSpecies_Click(object sender, RoutedEventArgs e)
        {
            var name = SpeciesNameTextBox.Text.Trim();
            var unit = SpeciesUnitTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var newSpecies = new Species { Name = name, Unit = string.IsNullOrEmpty(unit) ? null : unit };
                _context.Species.Add(newSpecies);
                _context.SaveChanges();
                LoadData();
            }
        }

        private void EditSpecies_Click(object sender, RoutedEventArgs e)
        {
            if (SpeciesGrid.SelectedItem is Species selected)
            {
                selected.Name = SpeciesNameTextBox.Text.Trim();
                selected.Unit = string.IsNullOrEmpty(SpeciesUnitTextBox.Text.Trim()) ? null : SpeciesUnitTextBox.Text.Trim();
                _context.Species.Update(selected);
                _context.SaveChanges();
                LoadData();
            }
        }
    }
}
