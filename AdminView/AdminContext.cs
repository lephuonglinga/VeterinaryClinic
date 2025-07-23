using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.AdminView
{
    class AdminContext
    {
        public static User? CurrentAdmin { get; set; }
    }
}
