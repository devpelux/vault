namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a report regarding the passwords.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; } = -1;

        /// <summary>
        /// Gets or sets the number of passwords.
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of duplicated passwords.
        /// </summary>
        public int Duplicated { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of weak passwords.
        /// </summary>
        public int Weak { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of old passwords.
        /// </summary>
        public int Old { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of violated passwords.
        /// </summary>
        public int Violated { get; set; } = 0;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public long Timestamp { get; set; } = -1;

        /// <summary>
        /// Initializes a new <see cref="Report"/> without id.
        /// </summary>
        public Report(int total, int duplicated, int weak, int old, int violated, long timestamp)
            : this(-1, total, duplicated, weak, old, violated, timestamp) { }

        /// <summary>
        /// Initializes a new <see cref="Report"/>.
        /// </summary>
        public Report(int id, int total, int duplicated, int weak, int old, int violated, long timestamp)
        {
            Id = id;
            Total = total;
            Duplicated = duplicated;
            Weak = weak;
            Old = old;
            Violated = violated;
            Timestamp = timestamp;
        }
    }
}
