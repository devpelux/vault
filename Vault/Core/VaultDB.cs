using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Vault.Core
{
    public sealed class VaultDB : IDisposable
    {
        public const int VersionNumber = 1;
        private readonly List<ITable> _tables = new();

        public static VaultDB Instance { get; private set; }

        public static VaultDBContext Context { get; set; }

        public SqliteConnection Connection { get; private set; }

        public Users Users => (Users)_tables[0];
        public Categories Categories => (Categories)_tables[1];
        public Passwords Passwords => (Passwords)_tables[2];
        public Cards Cards => (Cards)_tables[3];
        public Notes Notes => (Notes)_tables[4];


        private VaultDB()
        {
            try
            {
                OpenConnection();
                InitializeTables();
                UpdateDatabase(VersionNumber, GetVersion());
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        public static bool Initialize()
        {
            if (Instance != null) Instance.Dispose();
            try
            {
                Instance = new VaultDB();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _tables.Clear();
            CloseConnection();
            Instance = null;
        }

        public void ChangePassword(string newPassword)
        {
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT quote($newPassword);";
            command.Parameters.AddWithValue("$newPassword", newPassword);
            string quotedNewPassword = (string)command.ExecuteScalar();

            command.CommandText = "PRAGMA rekey = " + quotedNewPassword;
            command.Parameters.Clear();
            command.ExecuteNonQuery();
            Context = new VaultDBContext(Context.DatabaseFullPath, newPassword);
        }

        private void InitializeTables()
        {
            _tables.Add(new Users(this));
            _tables.Add(new Categories(this));
            _tables.Add(new Passwords(this));
            _tables.Add(new Cards(this));
            _tables.Add(new Notes(this));
        }

        private void OpenConnection()
        {
            if (Connection == null)
            {
                Directory.CreateDirectory(Context.DatabasePath);
                Connection = new SqliteConnection(Context.ConnectionString);
                Connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
        }

        private void UpdateDatabase(int newVersion, int oldVersion)
        {
            if (newVersion > oldVersion)
            {
                _tables.ForEach(table => table.UpdateTable(newVersion, oldVersion));
                SetVersion(newVersion);
            }
            else if (newVersion < oldVersion)
            {
                throw new FileFormatException("Non è possibile effettuare un downgrade.");
            }
        }

        #region Version

        private int GetVersion()
        {
            CheckVersionTable();
            string command = "SELECT VersionNumber FROM Version LIMIT 1;";
            SqliteCommand query = new SqliteCommand(command, Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            return reader.Read() ? reader.GetInt32(0) : 0;
        }

        private void SetVersion(int version)
        {
            string command = "INSERT OR REPLACE INTO Version (ID, VersionNumber) VALUES (@ID, @VersionNumber);";
            SqliteCommand query = new SqliteCommand(command, Connection);
            query.Parameters.AddWithValue("@VersionNumber", version);
            query.Parameters.AddWithValue("@ID", 1);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        private void CheckVersionTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS Version (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "VersionNumber INTEGER NOT NULL, " +
                                        "PRIMARY KEY(ID)" +
                                        ");";
            SqliteCommand query = new SqliteCommand(command, Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        #endregion
    }
}
