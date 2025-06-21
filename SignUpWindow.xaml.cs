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
using System.Windows.Shapes;
using VeterinaryClinic.Service;

namespace VeterinaryClinic
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private readonly UserService userService;
        public SignUpWindow()
        {
            userService = new UserService();
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            String username = UsernameTextBox.Text;
            String firstName = FirstNameTextBox.Text;
            String lastName = LastNameTextBox.Text;
            String phoneNumber = PhoneNumberTextBox.Text;
            String password = PasswordBox.Password;
            String confirmPassword = ConfirmPasswordBox.Password;
            string role = ClientRadio.IsChecked == true ? "Client" : "Doctor";

            bool isSignedUpSuccessfully = userService.SignUp(username, firstName, lastName, phoneNumber, role, password, confirmPassword);
            if (isSignedUpSuccessfully)
            {
                MessageBox.Show("Account created successfully!", "Sign Up", MessageBoxButton.OK, MessageBoxImage.Information);
                
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        
    }
}
