using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repository;

namespace VeterinaryClinic.Service
{
    internal class PatientService
    {
        PatientRepository patientRepository;

        public PatientService()
        {
            patientRepository = new PatientRepository();
        }

        public List<Patient>? GetAllPatientsByClient(Client client)
        {
            return patientRepository.GetAllPatientsByClient(client);
        }

    }
}
