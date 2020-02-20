using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Business.Core
{
    public static class PasswordHelper
    {
        private const int SALT_LENGTH = 20;
        private const int HASH_LENGTH = 16;
        private const int HASH_ITERATIONS = 12000;

        public static bool PasswordCompare(SecureString securePassword, string storedHash)
        {
            byte[] storedHashBytes = new byte[HASH_LENGTH];
            byte[] submittedPasswordBytes = new byte[HASH_LENGTH];
            string clearTextPassword = ConvertSecureString(securePassword);
            submittedPasswordBytes = Encoding.ASCII.GetBytes(clearTextPassword);
            storedHashBytes = Convert.FromBase64String(storedHash);

            byte[] storedSalt = ExtractSalt(storedHashBytes);
            byte[] submittedHashBytes = HashAndSalt(submittedPasswordBytes, storedSalt);

            if (Convert.ToBase64String(submittedHashBytes) != Convert.ToBase64String(storedHashBytes))
            {
                return false;
            }
            return true;
        }

        public static string GenerateNewHash(SecureString securePassword)
        {
            byte[] salt = new byte[SALT_LENGTH];
            RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider();
            saltGenerator.GetBytes(salt);

            byte[] submittedPasswordBytes = new byte[HASH_LENGTH];
            string clearTextPassword = ConvertSecureString(securePassword);
            submittedPasswordBytes = Encoding.ASCII.GetBytes(clearTextPassword);

            return Convert.ToBase64String(HashAndSalt(submittedPasswordBytes, salt));
        }

        private static byte[] ExtractSalt(byte[] storedHashBytes)
        {
            byte[] salt = new byte[SALT_LENGTH];
            try
            {
                Array.Copy(storedHashBytes, 0, salt, 0, SALT_LENGTH);
            }
            catch (Exception)
            {
            }
            return salt;
        }

        private static byte[] HashAndSalt(byte[] clearTextPasswordBytes, byte[] salt)
        {
            byte[] hashAndSalt = new byte[(SALT_LENGTH + HASH_LENGTH)];

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(clearTextPasswordBytes, salt, iterations: HASH_ITERATIONS);
            byte[] hashedPassword = pbkdf2.GetBytes(HASH_LENGTH);

            Array.Copy(salt, 0, hashAndSalt, 0, SALT_LENGTH);
            Array.Copy(hashedPassword, 0, hashAndSalt, SALT_LENGTH, HASH_LENGTH);

            return hashAndSalt;
        }

        private static string ConvertSecureString(SecureString securePassword)
        {
            #region Borrow Code: https://www.codeproject.com/Tips/549109/Working-with-SecureString

            IntPtr unmanagedPasswordString = IntPtr.Zero;
            try
            {
                unmanagedPasswordString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedPasswordString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordString);
            }

            #endregion Borrow Code: https://www.codeproject.com/Tips/549109/Working-with-SecureString
        }
    }
}