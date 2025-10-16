using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string Password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(Password, hashedPassword);
        }

        public static void PrintHashForWelcome01()
        {
            string password = "Welcome01";
            string hashed = HashPassword(password);
            Console.WriteLine($"Hash para Welcome01: {hashed}");
        }
    }
}
