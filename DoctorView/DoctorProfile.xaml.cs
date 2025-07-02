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
            currentDoctor = DoctorContext.CurrentDoctor;
            if(currentDoctor == null)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                DoctorDetailsWindow doctorDetailsWindow = DoctorDetailsWindow.GetInstance();
                doctorDetailsWindow.Close();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentDoctor != null)
            {                
            }
            else
            {
                MessageBox.Show("No doctor information available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
