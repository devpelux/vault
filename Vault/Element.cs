using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vault
{
    public class Element : IEquatable<Element>
    {
        public int ID { get; set; } = -1;
        public string Title { get; set; }
        public string Category { get; set; }
        public string Website { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Details { get; set; }

        public override bool Equals(object obj) => Equals(obj as Element);

        public bool Equals(Element other) => other != null && ID == other.ID;

        public override int GetHashCode() => 1213502048 + ID.GetHashCode();

        public static bool operator ==(Element left, Element right) => EqualityComparer<Element>.Default.Equals(left, right);

        public static bool operator !=(Element left, Element right) => !(left == right);
    }
}
