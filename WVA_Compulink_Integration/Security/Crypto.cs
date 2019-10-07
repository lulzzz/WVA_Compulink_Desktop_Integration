using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Security
{
    public class Crypto
    {
        public static string ConvertToHash(string inputString)
        {
            if (inputString == null || inputString.Length < 6)
                throw new Exception("'inputString' must be at least 6 characters");

            if (inputString.Length > 63)
                throw new Exception("'inputString' must not exceed 63 characters");

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Split the input string into a character array
                char[] letterArray = inputString.ToCharArray();

                // Create a salt based on the scrambled first 6 indexes of the inputString
                string salt = letterArray[0].ToString() + letterArray[4].ToString() + letterArray[2].ToString() + letterArray[5].ToString() + letterArray[1].ToString() + letterArray[3].ToString();

                // Combine the salt and inputString to create the first hash that will be used to create the final hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(salt + inputString));

                // Build the hash string from the 'bytes' array
                StringBuilder originalHash = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    originalHash.Append(bytes[i].ToString("x2"));

                // Define a string that is a chunk of the 'originalHash' based on the length of the input string         
                string hashChunk = originalHash.ToString().Substring(inputString.Length, originalHash.Length - inputString.Length);

                // Create the final hash using the salt and hash chunk
                bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(salt + hashChunk));

                // Build the hash string
                StringBuilder finalHash = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    finalHash.Append(bytes[i].ToString("x2"));

                // Return the hash
                return finalHash.ToString();
            }
        }

        private const string initVector = "deb3riagpR1pnh54";
        private const string passPhrase = "a3x2i7das5a";
        private const int keysize = 256;

        public static string Decrypt(string stringToDecrypt)
        {
            try
            {
                if (stringToDecrypt == null || stringToDecrypt.Trim() == "")
                    return stringToDecrypt;

                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(stringToDecrypt);
                var password = new PasswordDeriveBytes(passPhrase, null);
                password.IterationCount = 126;
                byte[] keyBytes = password.GetBytes(keysize / 8);
                var symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch
            {
                return stringToDecrypt;
            }
        }
    }
}
