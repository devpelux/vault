using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Vault.Core
{
    /// <summary>
    /// Provides utilities to encrypt data.
    /// </summary>
    public class Encryptor
    {
        /// <summary>
        /// Default hash size.
        /// </summary>
        public const int HASH_SIZE = 32;

        /// <summary>
        /// Default iteration count.
        /// </summary>
        public const int ITERATIONS = 1000;

        /// <summary>
        /// Generates a byte hash key from the specified secure string, with the specified salt size, iteration count, and size in byte number.
        /// </summary>
        public static byte[] GenerateKey(SecureString password, byte[] salt, int iterations = ITERATIONS, int hashSize = HASH_SIZE)
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

        /// <summary>
        /// Generates a random salt of the specified size in byte number.
        /// </summary>
        public static byte[] GenerateSalt(int saltSize = HASH_SIZE) => RandomNumberGenerator.GetBytes(saltSize);

        /// <summary>
        /// Converts a byte hash key array into a string.
        /// </summary>
        public static string ConvertToString(byte[] data) => Convert.ToBase64String(data);
    }
}
