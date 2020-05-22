using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CW2.Security
{
    public static class PasswordHelper
    {
        public static bool CheckPassword(string hash, string salt, string password)
        {
            return HashPassword(salt, password) == hash;
        }

        public static (string PasswordHash, string Salt) HashPassword(string password)
        {
            var salt = CreateSalt();
            return (HashPassword(salt, password), salt);
        }

        private static string HashPassword(string salt, string password)
        {
            var valueBytes = KeyDerivation.Pbkdf2(password,
                Encoding.UTF8.GetBytes(salt),
                KeyDerivationPrf.HMACSHA512,
                10000, 32);

            return Convert.ToBase64String(valueBytes);
        }

        private static string CreateSalt()
        {
            var randomBytes = new byte[16];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
