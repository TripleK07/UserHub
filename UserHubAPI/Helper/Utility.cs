using System;
using System.Security.Cryptography;

namespace UserHubAPI.Helper
{
    public class Utility
    {
        public static string GetCurrentApiUrl(HttpRequest httpRequest)
        {
            // Get the current API URL
            return $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}

