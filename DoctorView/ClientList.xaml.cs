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
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.DoctorView
{
    /// <summary>
    /// Interaction logic for ClientList.xaml
    /// </summary>
    public partial class ClientList : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        Doctor? currentDoctor = DoctorContext.CurrentDoctor;
        public ClientList()
        {
            InitializeComponent();
            if(currentDoctor == null)
            {
                MessageBox.Show("You are not logged in as a doctor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Page_Loaded();            
        }

        private void Page_Loaded()
        {
            ClientDataGrid.ItemsSource = context.Users.Include(u => u.Client)
                .ThenInclude(c => c.Patients)
                .ThenInclude(p => p.Cases)
                .Where(u => u.Client != null && u.Client.Patients.Any(p => p.Cases.Any(c => c.DoctorVcnNo == currentDoctor.VcnNo)))
                .ToList();

        }

        private void ClientDetail_Click(object sender, RoutedEventArgs e)
        {
            Button? senderButton = sender as Button;
            User? selectedUser = senderButton?.Tag as User;
            if (selectedUser == null)
            {
                MessageBox.Show("No user selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Client? client = selectedUser.Client;
            if (client == null)
            {
                MessageBox.Show("No client associated with this user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DetailStack.Visibility = Visibility.Visible;
            ClientId.Text = client.ClientId;
            Address.Text = client.Address ?? "No address provided.";
            FirstName.Text = $"{selectedUser.FirstName}";
            LastName.Text = $"{selectedUser.LastName}";
            PhoneNo.Text = selectedUser.PhoneNo ?? "No phone number provided.";
            Email.Text = selectedUser.Username;
            ViewPatients.Tag = selectedUser;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DetailStack.Visibility = Visibility.Collapsed;
            ClientId.Text = "";
            Address.Text = "";
            FirstName.Text = "";
            LastName.Text = "";
            PhoneNo.Text = "";
            Email.Text = "";
        }

        private void ViewPatients_Click(object sender, RoutedEventArgs e)
        {
            Button? senderButton = sender as Button;
            User? selectedUser = senderButton?.Tag as User;
            if (selectedUser == null)
            {
                MessageBox.Show("No user selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Client? client = selectedUser.Client;
            if (client == null)
            {
                MessageBox.Show("No client associated with this user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
            doctorDetailsWindow.MainFrame.Navigate(new PatientList(client));
        }
    }
}
