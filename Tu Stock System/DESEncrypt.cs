// Name: Colin Campbell
// Class: Graded Unit
// Project description: A stock system to be used within the clothing department of Sainsburys (Tu Clothing)
// Version: 1.00
// Date: 07/02/2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Tu_Stock_System
{
    class DESEncrypt
    {
        /// <summary>
        /// Generates key for encryption.
        /// </summary>
        /// <param name="key">The initial key to be used for encryption.</param>
        /// <returns></returns>
        static TripleDES CreateDES(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }

        /// <summary>
        /// Function used to encrypt a given string.
        /// </summary>
        /// <param name="plainText">The string that will be encrypted.</param>
        /// <param name="key">The key that will be used to further strengthen the encryption.</param>
        /// <returns>Returns an encrypted string.</returns>
        public string Encrypt(string plainText, string key)
        {
            // convert the plain text string into a byte array
            byte[] plainTextBytes = Encoding.Unicode.GetBytes(plainText);

            // use a memory stream to hold the bytes
            MemoryStream myStream = new MemoryStream();

            // create a key and initialization vector using a passed in key
            TripleDES des = CreateDES(key);

            // create the encoder that will write to the memory stream
            CryptoStream cryptStream = new CryptoStream(myStream, des.CreateEncryptor(), CryptoStreamMode.Write);

            // crypto stream will write the byte array to the memory stream
            cryptStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptStream.FlushFinalBlock();

            // convert the stream to an array and convert that back to a string so it can be returned
            return Convert.ToBase64String(myStream.ToArray());
        }

        /// <summary>
        /// Function used to decrypt a given string.
        /// </summary>
        /// <param name="encryptedText">Encrypted string to be decrypted.</param>
        /// <param name="key">The key that was used to help encrypt the string.</param>
        /// <returns>Returns a decrypted string.</returns>
        public string Decrypt(string encryptedText, string key)
        {
            // convert the encrypted text string into a byte array
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);

            // use a memory stream to hold the bytes
            MemoryStream myStream = new MemoryStream();

            // create a key and initialization vector using a passed in key
            TripleDES des = CreateDES(key);

            // create the decoder that will write to the memory stream
            CryptoStream decryptStream = new CryptoStream(myStream, des.CreateDecryptor(), CryptoStreamMode.Write);

            // crypto stream will write the byte array to the memory stream
            decryptStream.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);
            decryptStream.FlushFinalBlock();

            // convert the stream to a string to be returned
            return Encoding.Unicode.GetString(myStream.ToArray());
        }
    }
}
