using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SF.Common.Security
{
    internal class AESEncryption
    {
        #region Private Fields

        private readonly RijndaelManaged _algorithm;
        private readonly ICryptoTransform _decryptorTransform;
        private readonly UTF8Encoding _encoder;
        private readonly ICryptoTransform _encryptorTransform;

        #endregion Private Fields

        #region Public Constructors

        /// <param name="key">32 bytes</param>
        /// <param name="vector">16 bytes</param>
        /// <param name="salt">Minimum 8 characters</param>
        /// <param name="fast">true to perform faster encryption / decryption by performing less iterations (at the cost of producing less secure keys). This should be left as false wherever possible.</param>
        public AESEncryption(byte[] key, byte[] vector, string salt, bool fast = false)
        {
            if (key.Length != 32)
            {
                throw new ArgumentException(nameof(key), "AESEncryption key must be 32 bytes.");
            }

            if (vector.Length != 16)
            {
                throw new ArgumentException(nameof(vector), "AESEncryption vector must be 16 bytes.");
            }

            if (salt.Length < 8)
            {
                throw new ArgumentException(nameof(salt), "AESEncryption salt must be at least 8 characters.");
            }

            _encoder = new UTF8Encoding();

            _algorithm = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 256,
                BlockSize = 128
            };

            var derivedValue = new Rfc2898DeriveBytes(key, _encoder.GetBytes(salt), fast ? 9 : 779);

            _algorithm.Key = derivedValue.GetBytes(32);
            _algorithm.IV = vector;
            _encryptorTransform = _algorithm.CreateEncryptor();
            _decryptorTransform = _algorithm.CreateDecryptor();

            derivedValue.Reset();
        }

        #endregion Public Constructors

        #region Public Methods

        public static byte[] GenerateKey()
        {
            var oAlgorithm = new RijndaelManaged();

            oAlgorithm.GenerateKey();

            return oAlgorithm.Key;
        }

        public static byte[] GenerateVector()
        {
            var oAlgorithm = new RijndaelManaged();

            oAlgorithm.GenerateIV();

            return oAlgorithm.IV;
        }

        public string Decrypt(string encryptedValue)
        {
            string value = null;

            using (var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedValue)))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, _decryptorTransform, CryptoStreamMode.Read))
                {
                    using (var streamReader = new StreamReader(cryptoStream))
                    {
                        value = streamReader.ReadToEnd();
                    }
                }
            }

            return value;
        }

        public string Encrypt(string value)
        {
            string encryptedValue = null;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, _encryptorTransform, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(value);
                    }
                }

                encryptedValue = Convert.ToBase64String(memoryStream.ToArray());
            }

            return encryptedValue;
        }

        #endregion Public Methods
    }
}