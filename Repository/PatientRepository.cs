using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repository
{
    internal class PatientRepository
    {
        private readonly VeterinaryClinicContext context;
        public PatientRepository() => context = new VeterinaryClinicContext();
        public List<Patient>? GetAllPatientsByClient(Client client)
        {
            var result = context.Patients.ToList();
            return result;
        }
        
    }
}
