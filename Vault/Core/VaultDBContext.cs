using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Vault.Core
{
    public sealed class VaultDBContext : IEquatable<VaultDBContext>
    {
        public string DatabaseName { get; private set; }
        public string DatabasePath { get; private set; }
        public string DatabaseExtension { get; private set; }
        public string DatabaseFullPath { get; private set; }
        public string ConnectionString { get; private set; }


        public VaultDBContext(string databaseName, string databasePath, string databaseExtension, string password)
        {
            if (databaseName != "" && databasePath != "" && databaseExtension != "")
            {
                Initialize(databaseName, databasePath, databaseExtension, password);
            }
            else
            {
                throw new ArgumentException("Il nome del database, il percorso, e l'estensione, non possono essere vuoti.");
            }
        }

        public VaultDBContext(string databaseFullPath, string password)
        {
            if (databaseFullPath != "")
            {
                databaseFullPath = Path.GetFullPath(databaseFullPath);
                Initialize(Path.GetFileNameWithoutExtension(databaseFullPath), Path.GetDirectoryName(databaseFullPath), Path.GetExtension(databaseFullPath), password);
            }
            else
            {
                throw new ArgumentException("Il percorso non può essere vuoto.");
            }
        }

        private void Initialize(string databaseName, string databasePath, string databaseExtension, string password)
        {
            DatabaseName = databaseName;
            DatabasePath = Path.GetFullPath(databasePath);
            DatabaseExtension = databaseExtension;
            DatabaseFullPath = Path.Combine(DatabasePath, DatabaseName + DatabaseExtension);
            ConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = DatabaseFullPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = password
            }.ToString();
        }

        public override bool Equals(object obj) => Equals(obj as VaultDBContext);

        public bool Equals(VaultDBContext other) => other != null && ConnectionString == other.ConnectionString;

        public override int GetHashCode() => HashCode.Combine(ConnectionString);

        public static bool operator ==(VaultDBContext left, VaultDBContext right) => EqualityComparer<VaultDBContext>.Default.Equals(left, right);

        public static bool operator !=(VaultDBContext left, VaultDBContext right) => !(left == right);
    }
}
