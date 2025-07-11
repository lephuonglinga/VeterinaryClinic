using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows;
using System.CodeDom;
using System.Reflection.Emit;
using VeterinaryClinic.Models;
using VeterinaryClinic.Helpers;

namespace VeterinaryClinic.Service
{
    class UserService
    {
        private readonly Repository.UserRepository userRepository;

        public UserService()
        {
            userRepository = new Repository.UserRepository();
        }

        public User? Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            var user = userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return null;
            }
            if (!user.IsActive)
            {
                return null;
            }
            if(user.PasswordHash == UserService.HashPassword(password))
            {
                return user;
            }
            return null;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + "STATIC_SALT");
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool IsMatched(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }

        public bool SignUp(string username, string firstName, string lastName, string phone, string role, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phone))            
            {                
                MessageBox.Show("Fields cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);                
                return false;
            }
            if (!IsMatched(password, confirmPassword))
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Validation.Validate.IsValidEmail(username))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!Validation.Validate.IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var user = new Models.User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                PhoneNo = phone,
                Role = role,
                IsActive = true
            };
            try
            {
                userRepository.AddUser(user);
                if ("Client".Equals(user.Role))
                {
                    string clientId;
                    do
                    {
                        clientId = "CID-" + IdGenerator.GenerateUserId();
                    } while (userRepository.IsClientIdTaken(clientId));

                    var client = new Client
                    {
                        Username = user.Username,
                        ClientId = clientId,
                        Address = "(not set)",
                        UsernameNavigation = user
                    };
                    userRepository.AddClient(client);
                }
                if("Doctor".Equals(user.Role))
                {
                    string doctorId;
                    do
                    {
                        doctorId = "DID-" + IdGenerator.GenerateUserId();
                    } while (userRepository.IsClientIdTaken(doctorId));
                    var doctor = new Doctor
                    {
                        Username = user.Username,
                        VcnNo = doctorId,                        
                        UsernameNavigation = user
                    };
                    userRepository.AddDoctor(doctor);
                }
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error happened.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }                        
        }        





        }
}
