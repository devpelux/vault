namespace Vault.Core
{
    public class Card
    {
        public int ID { get; set; } = Cards.NewID;
        public int UserID { get; set; } = Cards.InvalidID;
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Details { get; set; } = "";
        public bool RequestKey { get; set; } = false;
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Number { get; set; } = "";
        public string SecureCode { get; set; } = "";
        public string Expiration { get; set; } = "";
        public string Note { get; set; } = "";
    }
}
