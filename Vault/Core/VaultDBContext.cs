using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Vault.Core
{
    public sealed class VaultDBContext : IEquatable<VaultDBContext>
    {
        public string DatabaseName { get; init; }
        public string DatabasePath { get; init; }
        public string DatabaseFullPath { get; init; }
        public string ConnectionString { get; init; }


        public VaultDBContext(string databaseName = "vault", string databasePath = ".")
        {
            if (databaseName == "" || databasePath == "") throw new ArgumentException("Il nome del database o il percorso non possono essere vuoti.");
            DatabaseName = databaseName;
            DatabasePath = Path.GetFullPath(databasePath);
            DatabaseFullPath = Path.Combine(DatabasePath, DatabaseName + ".db");
            ConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = DatabaseFullPath,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();
        }

        public override bool Equals(object obj) => Equals(obj as VaultDBContext);

        public bool Equals(VaultDBContext other) => other != null &&
                   DatabaseName == other.DatabaseName &&
                   DatabasePath == other.DatabasePath;

        public override int GetHashCode() => HashCode.Combine(DatabaseName, DatabasePath);

        public static bool operator ==(VaultDBContext left, VaultDBContext right) => EqualityComparer<VaultDBContext>.Default.Equals(left, right);

        public static bool operator !=(VaultDBContext left, VaultDBContext right) => !(left == right);
    }
}
