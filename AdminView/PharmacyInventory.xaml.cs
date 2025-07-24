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
using System.Collections.ObjectModel;

namespace VeterinaryClinic.AdminView
{
    /// <summary>
    /// Interaction logic for PharmacyInventory.xaml
    /// </summary>
    public partial class PharmacyInventory : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        ObservableCollection<VeterinaryClinic.Models.PharmacyInventory> inventories = new ObservableCollection<VeterinaryClinic.Models.PharmacyInventory>(); // Initialize to avoid null

        public PharmacyInventory()
        {
            InitializeComponent();
            LoadPharmacyInventory();
        }

        private void LoadPharmacyInventory()
        {
            if (context.PharmacyInventories != null)
            {
                inventories = new ObservableCollection<VeterinaryClinic.Models.PharmacyInventory>(context.PharmacyInventories.ToList());
                DgPharmacy.ItemsSource = inventories;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var inv in inventories)
                {
                    if (inv.ItemId == 0)
                    {
                        context.Add(inv);
                    }
                    else
                    {
                        context.Update(inv);
                    }
                }

                var dbIds = inventories.Select(e => e.ItemId).ToList();
                var toRemove = context.PharmacyInventories.Where(e => !dbIds.Contains(e.ItemId)).ToList();
                context.RemoveRange(toRemove);
                context.SaveChanges();
                MessageBox.Show("Update Success!");
                LoadPharmacyInventory();
            }
            catch (Exception)
            {
                MessageBox.Show("Error happened, try again!", "Error", MessageBoxButton.OK);
                return;
            }
        }
    }
}
