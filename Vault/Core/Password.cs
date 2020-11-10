namespace Vault.Core
{
    public class Password
    {
        public int ID { get; set; } = Passwords.NewID;
        public int UserID { get; set; } = Passwords.InvalidID;
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Details { get; set; } = "";
        public bool RequestKey { get; set; } = false;
        public string Website { get; set; } = "";
        public string Username { get; set; } = "";
        public string Key { get; set; } = "";
        public string Note { get; set; } = "";
    }
}
