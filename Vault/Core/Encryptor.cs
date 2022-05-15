using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Vault.Core
{
    public class Encryptor
    {
        public const int HASH_SIZE = 32;
        public const int ITERATIONS = 10000;

        public static byte[] GenerateKey(string password, byte[] salt, int iterations = ITERATIONS)
        {
            using Rfc2898DeriveBytes rfc2898 = new(password, salt, iterations, HashAlgorithmName.SHA256);
            return rfc2898.GetBytes(HASH_SIZE);
        }

        public static byte[] GenerateKey(byte[] password, byte[] salt, int iterations = ITERATIONS)
        {
            using Rfc2898DeriveBytes rfc2898 = new(password, salt, iterations, HashAlgorithmName.SHA256);
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
                    using Rfc2898DeriveBytes rfc2898 = new(passwordByteArray, salt, iterations, HashAlgorithmName.SHA256);
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
