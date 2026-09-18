using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Services
{
    public class PasswordService
    {


        public string HashPassword(string password)
        {
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(password)
            );
        }

        // Login password ko database password se match karna
        public bool VerifyPassword(string password, string passwordHash)
        {
            string hash = HashPassword(password);

            return hash == passwordHash;




        }
    }
}