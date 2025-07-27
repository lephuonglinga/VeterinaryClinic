using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace VeterinaryClinic.Validation
{
    class Validate
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            return true;
        }

        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            // Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, pattern);            
        }

        public static bool IsValidPhoneNo(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;
            
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9]{10,11}$");
        }


    }
}
