namespace Vault.Core
{
    public class Note
    {
        public int ID { get; set; } = Notes.NewID;
        public int UserID { get; set; } = Notes.InvalidID;
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Details { get; set; } = "";
        public bool RequestKey { get; set; } = false;
        public string Text { get; set; } = "";
    }
}
