using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repository;

namespace VeterinaryClinic.Service
{
    internal class DoctorService
    {
        DoctorRepository doctorRepository = new DoctorRepository();
        public DoctorService() { }

        public Doctor? GetDoctorByUser(User user)
        {
            if (user == null || !user.Role.Equals("Doctor"))
            {
                return null;
            }
            return doctorRepository.GetDoctorByUser(user);
        }
    }
}
