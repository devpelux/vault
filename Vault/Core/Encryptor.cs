using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Vault.Core
{
    public class Encryptor
    {
        public const int HASH_SIZE = 32;
        public const int ITERATIONS = 10000;


        public static string Encrypt(string str, byte[] key, byte[] iv = null)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            if (iv != null) aes.IV = iv;
            else aes.GenerateIV();
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(str);
            streamWriter.Close();
            return ConvertToString(aes.IV.Concat(memoryStream.ToArray()).ToArray());
        }

        public static string Encrypt(byte[] data, byte[] key, byte[] iv = null) => Encrypt(ConvertToString(data), key, iv);

        public static string Decrypt(string cipherIvStr, byte[] key)
        {
            byte[] cipherIvData = ConvertToBytes(cipherIvStr);
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = cipherIvData.Take(16).ToArray();
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream(cipherIvData.Skip(16).ToArray());
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public static byte[] GenerateKey(string password, byte[] salt, int iterations = ITERATIONS)
        {
            using Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            return rfc2898.GetBytes(HASH_SIZE);
        }

        public static byte[] GenerateKey(byte[] password, byte[] salt, int iterations = ITERATIONS)
        {
            using Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            return rfc2898.GetBytes(HASH_SIZE);
        }

        public static byte[] GenerateKey(SecureString password, byte[] salt, int iterations = ITERATIONS)
            => DeriveKey(password, salt, iterations, HASH_SIZE);

        public static byte[] GenerateKey() => GenerateSalt();

        public static byte[] GenerateSalt()
        {
            byte[] key = new byte[HASH_SIZE];
            new RNGCryptoServiceProvider().GetBytes(key);
            return key;
        }

        public static string ConvertToString(byte[] data) => Convert.ToBase64String(data);

        public static byte[] ConvertToBytes(string str) => Convert.FromBase64String(str);

        private static byte[] DeriveKey(SecureString password, byte[] salt, int iterations, int hashSize)
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToBSTR(password);
                int length = Marshal.ReadInt32(ptr, -4);
                byte[] passwordByteArray = new byte[length];
                GCHandle handle = GCHandle.Alloc(passwordByteArray, GCHandleType.Pinned);
                try
                {
                    for (int i = 0; i < length; i++) passwordByteArray[i] = Marshal.ReadByte(ptr, i);
                    using Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(passwordByteArray, salt, iterations, HashAlgorithmName.SHA256);
                    return rfc2898.GetBytes(hashSize);
                }
                finally
                {
                    Array.Clear(passwordByteArray, 0, passwordByteArray.Length);
                    handle.Free();
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero) Marshal.ZeroFreeBSTR(ptr);
            }
        }
    }
}
