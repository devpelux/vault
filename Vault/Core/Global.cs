namespace Vault.Core
{
    public class Global
    {
        private static Global _instance = null;
        public static Global Instance
        {
            get
            {
                if (_instance == null) _instance = new Global();
                return _instance;
            }
        }

        public int UserID { get; set; } = -1;
        public string Username { get; set; } = null;
        public byte[] Key { get; set; } = null;


        private Global() { }
    }
}
