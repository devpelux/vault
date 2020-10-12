using System;
using System.Collections.Generic;

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


        public Element() { }

        public Element(int id)
        {
            ID = id;
        }

        public Element(int id, string title, string category, string website, string username, string password, string details)
        {
            ID = id;
            Title = title;
            Category = category;
            Website = website;
            Username = username;
            Password = password;
            Details = details;
        }

        public Element(string title, string category, string website, string username, string password, string details) :
            this(-1, title, category, website, username, password, details)
        { }


        public override bool Equals(object obj) => Equals(obj as Element);

        public bool Equals(Element other) => other != null && ID == other.ID;

        public override int GetHashCode() => 1213502048 + ID.GetHashCode();

        public static bool operator ==(Element left, Element right) => EqualityComparer<Element>.Default.Equals(left, right);

        public static bool operator !=(Element left, Element right) => !(left == right);
    }
}
