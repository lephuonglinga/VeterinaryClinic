using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.DoctorView
{
    internal class DoctorContext
    {
        public static Doctor? CurrentDoctor { get; set; }
    }
}
