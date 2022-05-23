namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a card data.
    /// </summary>
    public class Card : Data
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cvv.
        /// </summary>
        public string Cvv { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the iban.
        /// </summary>
        public string? Iban { get; set; } = null;

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        public long Expiration { get; set; } = -1;

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public string? Notes { get; set; } = null;

        /// <inheritdoc/>
        public override string Header => Name;

        /// <inheritdoc/>
        public override string SubHeader => Utility.FormatHeader(Number, 4);

        /// <summary>
        /// Initializes a new <see cref="Card"/> without id.
        /// </summary>
        public Card(string category, string name, string owner, string number, string type, string cvv, string? iban, long expiration, string? notes, bool isLocked = false)
            : this(-1, category, name, owner, number, type, cvv, iban, expiration, notes, isLocked) { }

        /// <summary>
        /// Initializes a new <see cref="Card"/>.
        /// </summary>
        public Card(int id, string category, string name, string owner, string number, string type, string cvv, string? iban, long expiration, string? notes, bool isLocked = false)
            : base(id, category, isLocked)
        {
            Name = name;
            Owner = owner;
            Number = number;
            Type = type;
            Cvv = cvv;
            Iban = iban;
            Expiration = expiration;
            Notes = notes;
        }
    }
}
