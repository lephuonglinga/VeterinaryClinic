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
using VeterinaryClinic.DoctorView;

namespace VeterinaryClinic.Views
{
    /// <summary>
    /// Interaction logic for DoctorSidebar.xaml
    /// </summary>
    public partial class DoctorSidebar : UserControl
    {
        public DoctorSidebar()
        {
            InitializeComponent();
        }

        private void Client_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Client List window is not available.", new ClientList());
        }

        private void Patient_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Patient List window is not available.", new PatientList(null));
        }

        private void CheckRole(string message, Page page)
        {
            Doctor? user = DoctorContext.CurrentDoctor;
            DoctorDetailsWindow clientDetailsWindow = DoctorDetailsWindow.GetInstance();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Sp_DoctorSidebar.MaxWidth == 70)
            {
                Sp_DoctorSidebar.MaxWidth = 1000;
                Menu.Visibility = Visibility.Visible;
                Control.Content = "Hide";
            }
            else
            {
                HideSidebar();
            }
        }
        private void HideSidebar()
        {
            Sp_DoctorSidebar.MaxWidth = 70;
            Menu.Visibility = Visibility.Collapsed;
            Control.Content = "Show";
        }

        private void Cases_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Patient List window is not available.", new Cases(null));
        }

        private void Pharmacy_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Patient List window is not available.", new Pharmacy());
        }

        private void Prepscription_Selected(object sender, RoutedEventArgs e)
        {
            CheckRole("Patient List window is not available.", new Prescriptions(null));
        }
    }
}
