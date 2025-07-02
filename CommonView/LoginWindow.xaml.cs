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
using VeterinaryClinic.Models;
using VeterinaryClinic.DoctorView;


namespace VeterinaryClinic
{
    /// <summary>  
    /// Interaction logic for LoginWindow.xaml  
    /// </summary>  
    public partial class LoginWindow : Window
    {
        private readonly UserService userService;

        public LoginWindow()
        {
            userService = new UserService();
            InitializeComponent();
        }

        private void GoToSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            User? user = userService.Login(username, password);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
            MessageBox.Show("Login successful!", "Login", MessageBoxButton.OK, MessageBoxImage.Information);

            if ("Client".Equals(user.Role))
            {
                Client? client = new ClientService().GetClientByUser(user);
                if (client != null)
                {
                    ClientContext.CurrentClient = client;
                    ClientDetailsWindow clientDetailsWindow = ClientDetailsWindow.GetInstance();
                    clientDetailsWindow.MainFrame.Navigate(new ClientProfile());
                    clientDetailsWindow.Show();
                }                           
            }if ("Doctor".Equals(user.Role))
            {
                Doctor? doctor = new DoctorService().GetDoctorByUser(user);
                if (doctor != null)
                {
                    DoctorContext.CurrentDoctor = new DoctorService().GetDoctorByUser(user);
                    DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
                    doctorDetailsWindow.MainFrame.Navigate(new DoctorProfile());
                    doctorDetailsWindow.Show();
                }
               
                this.Close();
            }
        }
    }
}
