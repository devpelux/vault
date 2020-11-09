using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Vault.Core
{
    public class VaultDBContext : IEquatable<VaultDBContext>
    {
        public readonly string DatabaseName;
        public readonly string DatabasePath;
        public readonly string DatabaseFullPath;
        public readonly string ConnectionString;


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

        public override int GetHashCode()
        {
            int hashCode = -1505882814;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DatabaseName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DatabasePath);
            return hashCode;
        }

        public static bool operator ==(VaultDBContext left, VaultDBContext right) => EqualityComparer<VaultDBContext>.Default.Equals(left, right);

        public static bool operator !=(VaultDBContext left, VaultDBContext right) => !(left == right);
    }
}
