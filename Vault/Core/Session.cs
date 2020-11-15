namespace Vault.Core
{
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

        public int UserID { get; set; } = -1;
        public string Username { get; set; } = null;
        public byte[] Key { get; set; } = null;


        private Session() { }
    }
}
