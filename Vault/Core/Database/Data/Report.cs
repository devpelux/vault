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
        public long Total { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of duplicated passwords.
        /// </summary>
        public long Duplicated { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of weak passwords.
        /// </summary>
        public long Weak { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of old passwords.
        /// </summary>
        public long Old { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of violated passwords.
        /// </summary>
        public long Violated { get; set; } = 0;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public long Timestamp { get; set; } = -1;

        /// <summary>
        /// Initializes a new <see cref="Report"/> without id.
        /// </summary>
        public Report(long total, long duplicated, long weak, long old, long violated, long timestamp)
            : this(-1, total, duplicated, weak, old, violated, timestamp) { }

        /// <summary>
        /// Initializes a new <see cref="Report"/>.
        /// </summary>
        public Report(int id, long total, long duplicated, long weak, long old, long violated, long timestamp)
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
