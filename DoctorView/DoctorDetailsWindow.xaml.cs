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

namespace VeterinaryClinic
{
    /// <summary>
    /// Interaction logic for DoctorDetailsWindow.xaml
    /// </summary>
    public partial class DoctorDetailsWindow : Window
    {
        public DoctorDetailsWindow()
        {
            InitializeComponent();
        }

        private static DoctorDetailsWindow? _instance;

        public static DoctorDetailsWindow GetInstance()
        {
            if (_instance == null || !_instance.IsLoaded)
            {
                _instance = new DoctorDetailsWindow();
            }
            return _instance;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _instance = null; // Reset the instance when the window is closed
        }


    }
}
