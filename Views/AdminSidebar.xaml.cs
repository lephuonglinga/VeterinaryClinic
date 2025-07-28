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
using VeterinaryClinic.AdminView;

namespace VeterinaryClinic.Views
{
    /// <summary>
    /// Interaction logic for AdminSidebar.xaml
    /// </summary>
    public partial class AdminSidebar : UserControl
    {
        public AdminSidebar()
        {
            InitializeComponent();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = AdminWindow.GetInstance();
            if (adminWindow != null)
            {
                if (sender is ListBoxItem item)
                {
                    switch (item.Name)
                    {
                        case "DashboardItem":
                            adminWindow.MainFrame.Navigate(new AdminDashboardPage());
                            break;
                        case "DoctorListItem":
                            adminWindow.MainFrame.Navigate(new DoctorList());
                            break;
                        case "PharmacyInventoryItem":
                            adminWindow.MainFrame.Navigate(new PharmacyInventory());
                            break;
                        case "Prepscription":
                            adminWindow.MainFrame.Navigate(new AdminPrescriptionPage());
                            break;
                        case "PrescribedMed":
                            adminWindow.MainFrame.Navigate(new AdminPrescribedMedPage(null));
                            break;
                        default:
                            MessageBox.Show("Selected item is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Admin window is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
