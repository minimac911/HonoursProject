using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAM.Helpers
{
    public class PasswordHasher
    {
        private static string GenerateRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
            var salt = GenerateRandomSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool CheckHash(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
