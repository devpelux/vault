namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a password data.
    /// </summary>
    public class Password : Data
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public long Timestamp { get; set; } = -1;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? Username { get; set; } = null;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        public string? Notes { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating if the password is violated.
        /// </summary>
        public bool IsViolated { get; set; } = false;

        /// <inheritdoc/>
        public override string Header => Account;

        /// <inheritdoc/>
        public override string SubHeader => Utility.FormatHeader(Username, 6);

        /// <summary>
        /// Initializes a new <see cref="Password"/> without id.
        /// </summary>
        public Password(string category, string account, long timestamp, string? username, string value, string? notes, bool isViolated, bool isLocked = false)
            : this(-1, category, account, timestamp, username, value, notes, isViolated, isLocked) { }

        /// <summary>
        /// Initializes a new <see cref="Password"/>.
        /// </summary>
        public Password(int id, string category, string account, long timestamp, string? username, string value, string? notes, bool isViolated, bool isLocked = false)
            : base(id, category, isLocked)
        {
            Account = account;
            Timestamp = timestamp;
            Username = username;
            Value = value;
            Notes = notes;
            IsViolated = isViolated;
        }
    }
}
