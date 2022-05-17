using System;

namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a note data.
    /// </summary>
    public class Note : Data
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public long Timestamp { get; set; } = -1;

        /// <inheritdoc/>
        public override string Header => Title;

        /// <inheritdoc/>
        public override string SubHeader
        {
            get
            {
                DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
                string year = time.Year.ToString();
                string month = time.Month.ToString();
                string day = time.Day.ToString();
                string hour = time.Hour.ToString();
                string minute = time.Minute.ToString();

                return $"{day}/{month}/{year}  {hour}:{minute}";
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Note"/> without id.
        /// </summary>
        public Note(string category, string title, string text, long timestamp, bool isLocked = false)
            : this(-1, category, title, text, timestamp, isLocked) { }

        /// <summary>
        /// Initializes a new <see cref="Note"/>.
        /// </summary>
        public Note(int id, string category, string title, string text, long timestamp, bool isLocked = false)
            : base(id, category, isLocked)
        {
            Title = title;
            Text = text;
            Timestamp = timestamp;
        }
    }
}
