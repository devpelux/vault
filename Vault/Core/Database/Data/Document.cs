namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a document data.
    /// </summary>
    public class Document : Data
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        public string? Owner { get; set; } = null;

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string? Code { get; set; } = null;

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
        public override string SubHeader => Utility.FormatHeader(Code, 4);

        /// <summary>
        /// Initializes a new <see cref="Document"/> without id.
        /// </summary>
        public Document(string category, string name, string? owner, string? code, long expiration, string? notes, bool isLocked = false)
            : this(-1, category, name, owner, code, expiration, notes, isLocked) { }

        /// <summary>
        /// Initializes a new <see cref="Document"/>.
        /// </summary>
        public Document(int id, string category, string name, string? owner, string? code, long expiration, string? notes, bool isLocked = false)
            : base(id, category, isLocked)
        {
            Name = name;
            Owner = owner;
            Code = code;
            Expiration = expiration;
            Notes = notes;
        }
    }
}
