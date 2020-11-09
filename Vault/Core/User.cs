using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public class User : IEquatable<User>
    {
        public const int NewID = -1;

        public int ID { get; set; } = NewID;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Key { get; set; } = "";


        public override bool Equals(object obj) => Equals(obj as User);
        public bool Equals(User other) => other != null && ID == other.ID;
        public override int GetHashCode() => 1213502048 + ID.GetHashCode();

        public static bool operator ==(User left, User right) => EqualityComparer<User>.Default.Equals(left, right);
        public static bool operator !=(User left, User right) => !(left == right);
    }
}
