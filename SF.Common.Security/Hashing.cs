using System;
using System.Security.Cryptography;

namespace SF.Common.Security
{
    public static class Hashing
    {
        private const int HASH_BYTE_SIZE = 32;
        private const int ITERATION_INDEX = 0;
        private const int PBKDF2_INDEX = 2;
        private const int PBKDF2_ITERATIONS = 1000;
        private const int SALT_BYTE_SIZE = 32;
        private const int SALT_INDEX = 1;

        public static string Hash(string stringToHash)
        {
            if (string.IsNullOrWhiteSpace(stringToHash))
            {
                throw new ArgumentException(nameof(stringToHash), "String To Hash can not be null, empty or whitespace.");
            }

            var salt = new byte[SALT_BYTE_SIZE];

            (new RNGCryptoServiceProvider()).GetBytes(salt);

            var hash = PBKDF2(stringToHash, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);

            return PBKDF2_ITERATIONS + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public static bool Validate(string plainString, string hashedString)
        {
            if (string.IsNullOrWhiteSpace(plainString))
            {
                throw new ArgumentException(nameof(plainString), "Plain String can not be null, empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(hashedString))
            {
                throw new ArgumentException(nameof(hashedString), "Hashed String can not be null, empty or whitespace.");
            }

            var delimiter = new char[] { ':' };
            var split = hashedString.Split(delimiter);
            var iterations = Int32.Parse(split[ITERATION_INDEX]);
            var salt = Convert.FromBase64String(split[SALT_INDEX]);
            var hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
            var testHash = PBKDF2(plainString, salt, iterations, hash.Length);

            return SlowEquals(hash, testHash);
        }

        private static byte[] PBKDF2(string plainString, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(plainString, salt)
            {
                IterationCount = iterations,
            };

            return pbkdf2.GetBytes(outputBytes);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;

            for (var i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }
    }
}