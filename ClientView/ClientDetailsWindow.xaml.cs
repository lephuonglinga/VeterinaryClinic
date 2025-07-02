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
using VeterinaryClinic.Models;

namespace VeterinaryClinic
{
    /// <summary>
    /// Interaction logic for ClientDetailsWindow.xaml
    /// </summary>
    public partial class ClientDetailsWindow : Window
    {
        private static ClientDetailsWindow? _instance;
        public ClientDetailsWindow()
        {
            InitializeComponent();            
        }

        public static ClientDetailsWindow GetInstance()
        {
            if (_instance == null || !_instance.IsLoaded)
            {
                _instance = new ClientDetailsWindow();
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
