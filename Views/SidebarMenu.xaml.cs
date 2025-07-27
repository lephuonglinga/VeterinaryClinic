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
using VeterinaryClinic.ClientView;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Views
{
    /// <summary>
    /// Interaction logic for SidebarMenu.xaml
    /// </summary>
    public partial class SidebarMenu : UserControl
    {
        public SidebarMenu()
        {
            InitializeComponent();
        }

        private void Profile_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Client details window is not available.", new ClientProfile());
        }

        private void MyPets_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Client details window is not available.", new MyPets());
        }

        private void CheckRole(string message, Page page)
        {
            Client? user = ClientContext.CurrentClient;
            ClientDetailsWindow clientDetailsWindow = ClientDetailsWindow.GetInstance();

            if (clientDetailsWindow != null)
            {
                clientDetailsWindow.MainFrame.Navigate(page);
                clientDetailsWindow.Show();
            }
            else
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Client details window is not available.", new ClientPrescriptionPage(null));
        }
    }
}
