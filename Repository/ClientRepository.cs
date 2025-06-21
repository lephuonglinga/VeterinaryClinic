using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repository
{
    internal class ClientRepository
    {
        private readonly VeterinaryClinicContext context;
        public ClientRepository() {
            context = new VeterinaryClinicContext();
        }   
        public Client GetClientByUser(User user)
        {
            var client = context.Clients.FirstOrDefault(c => c.Username == user.Username);
            if (client == null)
            {
                throw new Exception("Client not found for the given user.");
            }
            return client;
        }
    }
}
