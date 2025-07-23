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
    /// Interaction logic for PharmacyInventory.xaml
    /// </summary>
    public partial class PharmacyInventory : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public PharmacyInventory()
        {
            InitializeComponent();
            LoadPharmacyInventory();
        }

        private void LoadPharmacyInventory()
        {
            DgPharmacy.ItemsSource = context.PharmacyInventories.ToList();
        }
    }
}
