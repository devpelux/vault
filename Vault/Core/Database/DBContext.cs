using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Vault.Core.Database
{
    /// <summary>
    /// Contains informations about the connection to the database.
    /// </summary>
    public sealed class DBContext : IEquatable<DBContext?>
    {
        /// <summary>
        /// Gets the path of the database.
        /// </summary>
        public string DatabasePath { get; }

        /// <summary>
        /// Gets the connection string for connecting to the database.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DBContext"/> with the database path and the password.
        /// </summary>
        /// <param name="databasePath">Path of the database.</param>
        /// <param name="password">Password of the database.</param>
        /// <exception cref="ArgumentException"></exception>
        public DBContext(string databasePath, string password)
        {
            if (databasePath != "")
            {
                DatabasePath = Path.GetFullPath(databasePath);
                ConnectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = DatabasePath,
                    Mode = SqliteOpenMode.ReadWriteCreate,
                    Password = password
                }.ToString();
            }
            else
            {
                throw new ArgumentException("The database path cannot be empty.");
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as DBContext);

        /// <inheritdoc/>
        public bool Equals(DBContext? other) => other != null && DatabasePath == other.DatabasePath && ConnectionString == other.ConnectionString;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(DatabasePath, ConnectionString);
    }
}
