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
using Microsoft.EntityFrameworkCore;

namespace VeterinaryClinic
{
    /// <summary>
    /// Interaction logic for ClientProfile.xaml
    /// </summary>
    public partial class ClientProfile : Page
    {
        private readonly Client? currentClient;
        private readonly User? linkedUser;
        VeterinaryClinicContext context = new VeterinaryClinicContext();

        public ClientProfile()
        {
            Client? client = ClientContext.CurrentClient;
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client cannot be null");
            }
            InitializeComponent();
            currentClient = context.Clients.Include(c => c.UsernameNavigation).FirstOrDefault(c => c.Username == client.Username);
            
            
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("All fields are required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Validation.Validate.IsValidPhoneNo(phone))
            {
                MessageBox.Show("Invalid phone number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (currentClient != null)
            {
                currentClient.Address = address;
                currentClient.UsernameNavigation.FirstName = firstName;
                currentClient.UsernameNavigation.LastName = lastName;
                currentClient.UsernameNavigation.PhoneNo = phone;                
            }
            using (var context = new VeterinaryClinicContext())
            {                
                if (currentClient != null)
                {
                    context.Clients.Update(currentClient);
                }
                context.SaveChanges();
                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
