using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

namespace VeterinaryClinic.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorProfile.xaml
    /// </summary>
    public partial class DoctorProfile : Page
    {
        private readonly Doctor? currentDoctor;
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public DoctorProfile()
        {
            InitializeComponent();
            Doctor doctor = DoctorContext.CurrentDoctor;
            if (doctor == null)
            {
                MessageBox.Show("No doctor information available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            currentDoctor = context.Doctors.Include(d => d.UsernameNavigation).FirstOrDefault(d => d.Username == doctor.Username);
            if (currentDoctor == null)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
                doctorDetailsWindow.Close();
            }
            else
            {
                Page_Loaded();
            }
                
        }

        private void Page_Loaded()
        {
            if (currentDoctor != null)
            {
                txtFirstName.Text = currentDoctor.UsernameNavigation.FirstName;
                txtLastName.Text = currentDoctor.UsernameNavigation.LastName;
                txtEmail.Text = currentDoctor.UsernameNavigation.Username;
                txtPhone.Text = currentDoctor.UsernameNavigation.PhoneNo;
                txtYear.Text = currentDoctor.YearGraduated + "";
                txtVcn.Text = currentDoctor.VcnNo;
            }
            else
            {
                MessageBox.Show("No doctor information available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string year = txtYear.Text.Trim();

            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Fill all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentDoctor != null)
            {
                if (int.TryParse(year, out int yearGraduated) && yearGraduated > 1900)
                {
                    currentDoctor.YearGraduated = yearGraduated;
                }
                else
                {
                    MessageBox.Show("Year graduated must be a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Validation.Validate.IsValidPhoneNo(phone))
                {
                    MessageBox.Show("Invalid phone number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                currentDoctor.UsernameNavigation.FirstName = firstName;
                currentDoctor.UsernameNavigation.LastName = lastName;
                currentDoctor.UsernameNavigation.PhoneNo = phone;
            }

            using (var context = new VeterinaryClinicContext())
            {
                if (currentDoctor != null)
                {
                    context.Doctors.Update(currentDoctor);
                }
                context.SaveChanges();
                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
