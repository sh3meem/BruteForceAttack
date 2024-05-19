using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordResetApp.Models
{
    public static class PasswordManager
    {
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("constantSalt");

        public static string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password);
                var saltedPasswordWithSalt = new byte[saltedPassword.Length + Salt.Length];

                Buffer.BlockCopy(saltedPassword, 0, saltedPasswordWithSalt, 0, saltedPassword.Length);
                Buffer.BlockCopy(Salt, 0, saltedPasswordWithSalt, saltedPassword.Length, Salt.Length);

                var hash = sha256.ComputeHash(saltedPasswordWithSalt);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
