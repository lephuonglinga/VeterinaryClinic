using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repository;

namespace VeterinaryClinic.Service
{
    internal class ClientService
    {
        ClientRepository clientRepository;

        public ClientService()
        {
            this.clientRepository = new ClientRepository();
        }

        public Client? GetClientByUser(User user)
        {
            return clientRepository.GetClientByUser(user);
        }
    }
}
