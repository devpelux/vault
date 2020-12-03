﻿namespace Vault.Core
{
    public record CategoriesWindowData (double Width, double Height);

    public sealed class Session
    {
        private static Session _instance = null;
        public static Session Instance
        {
            get
            {
                if (_instance == null) _instance = new Session();
                return _instance;
            }
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public byte[] Key { get; set; }

        public CategoriesWindowData CategoriesWindowData { get; set; }


        private Session() => Clear();

        public void Clear()
        {
            UserID = -1;
            Username = null;
            Key = null;
            CategoriesWindowData = null;
        }
    }
}
