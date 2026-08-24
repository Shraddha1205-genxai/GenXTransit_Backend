using GenXTransitAPI.DataAccess.Interface.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Security
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            try
            {
                string[] parts = passwordHash.Split('.');

                if (parts.Length != 3)
                    return false;

                int iterations = int.Parse(parts[0]);

                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] storedHash = Convert.FromBase64String(parts[2]);

                byte[] enteredHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    storedHash.Length);

                return CryptographicOperations.FixedTimeEquals(
                    enteredHash,
                    storedHash);
            }
            catch
            {
                return false;
            }
        }

        public string GenerateTemporaryPassword()
        {
            const string upperChars = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijkmnopqrstuvwxyz";
            const string numbers = "23456789";
            const string specialChars = "@#$%&*";

            var random = RandomNumberGenerator.Create();

            char GetRandomChar(string chars)
            {
                var buffer = new byte[1];
                random.GetBytes(buffer);

                return chars[buffer[0] % chars.Length];
            }

            var password = new char[12];

            password[0] = GetRandomChar(upperChars);
            password[1] = GetRandomChar(lowerChars);
            password[2] = GetRandomChar(numbers);
            password[3] = GetRandomChar(specialChars);

            const string allChars =
                upperChars + lowerChars + numbers + specialChars;

            for (int i = 4; i < password.Length; i++)
            {
                password[i] = GetRandomChar(allChars);
            }

            // Shuffle password so required characters aren't always at the beginning
            return new string(password.OrderBy(_ => Guid.NewGuid()).ToArray());
        }
    }
}
    