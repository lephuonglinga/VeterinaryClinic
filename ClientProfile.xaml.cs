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

namespace VeterinaryClinic
{
    /// <summary>
    /// Interaction logic for ClientProfile.xaml
    /// </summary>
    public partial class ClientProfile : Page
    {
        private readonly Client currentClient;
        private readonly User? linkedUser;

        public ClientProfile(Client client)
        {
            InitializeComponent();
            currentClient = client;

            using var context = new VeterinaryClinicContext();

            // Load User theo ClientId
            linkedUser = context.Users
                .FirstOrDefault(u => u.Client != null && u.Client.ClientId == client.ClientId);

            // Gán thông tin lên giao diện
            if (linkedUser != null)
            {
                txtFirstName.Text = linkedUser.FirstName;
                txtLastName.Text = linkedUser.LastName;
                txtEmail.Text = linkedUser.Username; // hoặc Email nếu có
                txtPhone.Text = linkedUser.PhoneNo;
            }

            // Gán thông tin client
            txtAddress.Text = currentClient.Address;            
        }

        private void SidebarMenu_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Sidebar loaded");
        }
    }
}
