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
        private readonly Client? currentClient;
        private readonly User? linkedUser;

        public ClientProfile()
        {
            Client? client = ClientContext.CurrentClient;
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client cannot be null");
            }
            InitializeComponent();
            currentClient = client;

            using var context = new VeterinaryClinicContext();
            
            linkedUser = context.Users
                .FirstOrDefault(u => u.Client != null && u.Client.ClientId == client.ClientId);
            
            if (linkedUser != null)
            {
                txtFirstName.Text = linkedUser.FirstName;
                txtLastName.Text = linkedUser.LastName;
                txtEmail.Text = linkedUser.Username;
                txtPhone.Text = linkedUser.PhoneNo;
            }
            
            txtAddress.Text = currentClient.Address;            
        }        
    }
}
