using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Repository
{    
class UserRepository
    {
        private readonly VeterinaryClinicContext context;
        public UserRepository() => context = new VeterinaryClinicContext();

        public User? GetUserByUsername(string username)
        {
            return context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public bool IsClientIdTaken(string clientId)
        {
            return context.Clients.Any(c => c.ClientId == clientId);
        }

        public void AddClient(Client client)
        {
            context.Clients.Add(client);
            context.SaveChanges();
        }
    }
}
