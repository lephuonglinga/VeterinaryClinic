using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repository
{
    class DoctorRepository
    {
        private readonly VeterinaryClinicContext context;
        public DoctorRepository()
        {
            context = new VeterinaryClinicContext();
        }
        public Doctor GetDoctorByUser(User user)
        {
            var doctor = context.Doctors.FirstOrDefault(d => d.Username == user.Username);
            if (doctor == null)
            {
                throw new Exception("Doctor not found for the given user.");
            }
            return doctor;
        }
    }
}
