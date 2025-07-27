using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
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
using VeterinaryClinic.Models;
using VeterinaryClinic.Service;

namespace VeterinaryClinic.CommonView
{
    /// <summary>
    /// Interaction logic for ResetPassword.xaml
    /// </summary>
    public partial class ResetPassword : Window
    {
        private string _generatedCode;
        private string _userEmail;
        private User user;
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void BtnSendCode_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                MessageBox.Show("Please enter email");
                return;
            }

            _userEmail = EmailBox.Text.Trim();
            var foundUser = context.Users.FirstOrDefault(u => u.Username.Equals(_userEmail));

            if (foundUser == null)
            {
                MessageBox.Show("Email not found");
                return;
            }
            user = foundUser;

            // Generate OTP code (6 digits)
            Random rnd = new Random();
            _generatedCode = rnd.Next(100000, 999999).ToString();

            try
            {
                SendEmail(_userEmail, _generatedCode);
                MessageBox.Show("Code was sent to email");
                OtpBox.Visibility = Visibility.Visible;
                BtnVerifyCode.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void BtnVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            if (OtpBox.Text.Trim() == _generatedCode)
            {
                MessageBox.Show("Verify code is valid. Enter new password");
                NewPasswordBox.Visibility = Visibility.Visible;
                BtnSavePassword.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Wrong code, verified fail");
            }
        }

        private void BtnSavePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPass = NewPasswordBox.Password;
            if (string.IsNullOrWhiteSpace(newPass))
            {
                MessageBox.Show("Please enter new password");
                return;
            }

            // Thực tế: Lưu mật khẩu mới vào database            
            user.PasswordHash = HashPassword(newPass);
            context.Update(user);
            context.SaveChanges();
            MessageBox.Show($"New password for {_userEmail} has been updated");
            this.Close();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + "STATIC_SALT");
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private void SendEmail(string toEmail, string code)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("lephuonglinhnga1801@gmail.com", "ccnb hkhe clpv rpta"),
                EnableSsl = true
            };

            smtp.Send("lephuonglinhnga1801@gmail.com", toEmail, "Verify your account", $"Your code is: {code}");
        }
    }
}
