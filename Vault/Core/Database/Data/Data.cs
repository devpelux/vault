namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a data contained in the database.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; } = -1;

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating if the data is locked.
        /// </summary>
        public bool IsLocked { get; set; } = false;

        #region Header and SubHeader

        /// <summary>
        /// Gets the header representing the data.
        /// </summary>
        public virtual string Header => string.Empty;

        /// <summary>
        /// Gets the sub-header representing the data.
        /// </summary>
        public virtual string SubHeader => string.Empty;

        #endregion

        /// <summary>
        /// Initializes a new <see cref="Data"/>.
        /// </summary>
        public Data(int id, string category, bool isLocked = false)
        {
            Id = id;
            Category = category;
            IsLocked = isLocked;
        }
    }
}
