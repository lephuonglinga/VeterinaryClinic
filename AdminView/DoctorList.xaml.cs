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

namespace VeterinaryClinic.AdminView
{
    /// <summary>
    /// Interaction logic for DoctorList.xaml
    /// </summary>
    public partial class DoctorList : Page
    {
        VeterinaryClinicContext context = new VeterinaryClinicContext();
        public DoctorList()
        {
            InitializeComponent();
            Page_Loaded();
        }

        private void Page_Loaded()
        {
            DoctorDataGrid.ItemsSource = context.Doctors
                .Include(d => d.UsernameNavigation)
                .Select(d => new
                {
                    d.Username,
                    d.VcnNo,
                    d.Email,
                    d.YearGraduated,
                    FullName = $"{d.UsernameNavigation.FirstName} {d.UsernameNavigation.LastName}",
                    IsActive = d.UsernameNavigation.IsActive,
                    PhoneNo = d.UsernameNavigation.PhoneNo,
                })
                .ToList();
        }
        
    }
}
