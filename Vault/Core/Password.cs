using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public class Password : IEquatable<Password>
    {
        public const int NewID = -1;
        public const int InvalidID = -1;

        public int ID { get; set; } = NewID;
        public int UserID { get; set; } = InvalidID;
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Website { get; set; } = "";
        public string Username { get; set; } = "";
        public string Key { get; set; } = "";
        public string Details { get; set; } = "";
        public string Note { get; set; } = "";
        public bool RequestKey { get; set; } = false;


        public static Password Encrypt(Password password, byte[] key)
        {
            if (password != null)
            {
                return new Password
                {
                    ID = password.ID,
                    UserID = password.UserID,
                    Title = password.Title,
                    Category = password.Category,
                    Website = password.Website,
                    Username = Encryptor.Encrypt(password.Username, key),
                    Key = Encryptor.Encrypt(password.Key, key),
                    Details = password.Details,
                    Note = Encryptor.Encrypt(password.Note, key),
                    RequestKey = password.RequestKey
                };
            }
            return null;
        }
        public static Password Decrypt(Password encryptedPassword, byte[] key)
        {
            if (encryptedPassword != null)
            {
                return new Password
                {
                    ID = encryptedPassword.ID,
                    UserID = encryptedPassword.UserID,
                    Title = encryptedPassword.Title,
                    Category = encryptedPassword.Category,
                    Website = encryptedPassword.Website,
                    Username = Encryptor.Decrypt(encryptedPassword.Username, key),
                    Key = Encryptor.Decrypt(encryptedPassword.Key, key),
                    Details = encryptedPassword.Details,
                    Note = Encryptor.Decrypt(encryptedPassword.Note, key),
                    RequestKey = encryptedPassword.RequestKey
                };
            }
            return null;
        }

        public override bool Equals(object obj) => Equals(obj as Password);
        public bool Equals(Password other) => other != null && ID == other.ID;
        public override int GetHashCode() => 1213502048 + ID.GetHashCode();

        public static bool operator ==(Password left, Password right) => EqualityComparer<Password>.Default.Equals(left, right);
        public static bool operator !=(Password left, Password right) => !(left == right);
    }
}
