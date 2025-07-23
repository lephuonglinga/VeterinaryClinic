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

namespace VeterinaryClinic.AdminView
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private static AdminWindow? _instance;
        public AdminWindow()
        {
            InitializeComponent();
        }

        public static AdminWindow GetInstance()
        {
            if (_instance == null || !_instance.IsLoaded)
            {
                _instance = new AdminWindow();
            }

            return _instance;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _instance = null;
        }
    }
}
