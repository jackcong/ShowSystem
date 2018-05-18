using System;
using System.Security.Cryptography;
using System.Text;

namespace ComLib.Security
{
    /// <summary>
    /// Hashing class. Only SHA1 is supported.
    /// </summary>
    public static class Hashing
    {
        private static byte[] GetSalt()
        {
            var salt = new byte[9];
            var rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetBytes(salt);
            return salt;
        }

        private static string HashCode(string input, string salt)
        {
            byte[] mixed = Encoding.UTF8.GetBytes((input + salt).ToCharArray());
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] hashedStr = algorithm.ComputeHash(mixed);
            return Convert.ToBase64String(hashedStr);
        }

        /// <summary>
        /// Gets the SHA1 hash code of the given input + salt.
        /// </summary>
        /// <param name="input">The code to hash.</param>
        /// <param name="salt">The salt.
        /// 1) If is null or empty, will get a random salt and use it.
        /// 2) Else will use this salt.
        /// </param>
        /// <returns>The hashed string.</returns>
        private static string Hash(string input, ref string salt)
        {
            if (String.IsNullOrEmpty(salt))
            {
                salt = Convert.ToBase64String(GetSalt());
            }

            return HashCode(input, salt);
        }

        /// <summary>
        /// Gets the SHA1 hash code of the given input and salt.
        /// </summary>
        /// <param name="input">The code to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The hashed string.</returns>
        public static string HashWithSalt(this string input, string salt)
        {
            return Hash(input, ref salt);
        }

        /// <summary>
        /// Gets the SHA1 hash code of the given input with a random 9-byte salt.
        /// </summary>
        /// <param name="input">The code to hash.</param>
        /// <param name="salt">Output the random salt.</param>
        /// <returns>The hashed string.</returns>
        public static string HashWithoutSalt(this string input, out string salt)
        {
            salt = String.Empty;
            return Hash(input, ref salt);
        }
    }
}
