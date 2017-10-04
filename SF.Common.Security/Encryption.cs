using System;
using System.Text;

namespace SF.Common.Security
{
    public class Encryption
    {
        private readonly AESEncryption _AESEncryption;

        public Encryption(
            string encryptionKey,
            string encryptionSalt,
            byte[] encryptionVector)
        {
            if (string.IsNullOrWhiteSpace(encryptionKey))
            {
                throw new ArgumentException(nameof(encryptionKey), "Encryption Key can not be null, empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(encryptionSalt))
            {
                throw new ArgumentException(nameof(encryptionSalt), "Encryption Salt can not be null, empty or whitespace.");
            }

            if (encryptionVector == null)
            {
                throw new ArgumentNullException(nameof(encryptionVector), "Encryption Vector can not be null.");
            }

            var encryptionKeyArray = Encoding.ASCII.GetBytes(encryptionKey);

            _AESEncryption = new AESEncryption(
                encryptionKeyArray,
                encryptionVector,
                encryptionSalt);
        }

        public string DecryptString(string stringToDecrypt)
        {
            if (string.IsNullOrWhiteSpace(stringToDecrypt))
            {
                throw new ArgumentException(nameof(stringToDecrypt), "String To Decrypt can not be null, empty or whitespace.");
            }

            return _AESEncryption.Decrypt(stringToDecrypt);
        }

        public string EncryptString(string stringToEncrypt)
        {
            if (string.IsNullOrWhiteSpace(stringToEncrypt))
            {
                throw new ArgumentException(nameof(stringToEncrypt), "String To Encrypt can not be null, empty or whitespace.");
            }

            return _AESEncryption.Encrypt(stringToEncrypt);
        }
    }
}