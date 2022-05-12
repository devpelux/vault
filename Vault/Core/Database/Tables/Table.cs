namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Defines a table with his essential methods such as create, delete, update.
    /// </summary>
    public abstract class Table
    {
        /// <summary>
        /// Database instance.
        /// </summary>
        protected DB DB { get; }

        /// <summary>
        /// Initializes a new table.
        /// </summary>
        public Table(DB db) => DB = db;

        /// <summary>
        /// Creates the table.
        /// </summary>
        public abstract void Create();

        /// <summary>
        /// Deletes the table.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Updates the table from the old version to the new version.
        /// </summary>
        /// <param name="newVersion">New version of the table.</param>
        /// <param name="oldVersion">Old version of the table.</param>
        public abstract void Update(int newVersion, int oldVersion);
    }
}
