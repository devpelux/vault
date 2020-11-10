using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Vault.Core
{
    public partial class VaultDB : IDisposable
    {
        private static VaultDB _instance = null;
        public static VaultDB Instance
        {
            get
            {
                if (_instance == null) _instance = new VaultDB();
                return _instance;
            }
        }

        private const int VERSION = 1;

        private static VaultDBContext _context = new VaultDBContext("vault", ".");
        public static VaultDBContext Context
        {
            get => _context;
            set
            {
                if (value != null && value != _context)
                {
                    _context = value;
                    if (_instance != null) _instance.LoadVault();
                }
            }
        }

        public SqliteConnection Connection { get; private set; } = null;

        public Users Users { get; private set; } = null;
        public Passwords Passwords { get; private set; } = null;
        public Cards Cards { get; private set; } = null;
        public Notes Notes { get; private set; } = null;


        private VaultDB()
        {
            LoadVault();
        }

        private void LoadVault()
        {
            Users = new Users(this);
            Passwords = new Passwords(this);
            Cards = new Cards(this);
            Notes = new Notes(this);
            CloseConnection();
            OpenConnection();
            UpdateDatabase(VERSION, GetVersion());
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
                Users.UpdateTable(newVersion, oldVersion);
                Passwords.UpdateTable(newVersion, oldVersion);
                Cards.UpdateTable(newVersion, oldVersion);
                Notes.UpdateTable(newVersion, oldVersion);
                SetVersion(newVersion);
            }
            else if (newVersion < oldVersion)
            {
                throw new FileFormatException("Non è possibile effettuare un downgrade.");
            }
        }

        public void Dispose()
        {
            CloseConnection();
            _instance = null;
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
