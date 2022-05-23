using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using Vault.Core.Database.Data;
using Vault.Core.Database.Tables;

namespace Vault.Core.Database
{
    /// <summary>
    /// Singleton that manages the database connection and interactions.
    /// </summary>
    public sealed class DB : IDisposable
    {
        /// <summary>
        /// Gets the version of the database manager.
        /// </summary>
        public const int Version = 1;

        /// <summary>
        /// Gets or sets the informations about the connection to the database.
        /// </summary>
        public static DBContext? Context { get; set; }

        /// <summary>
        /// Gets the current informations about the connection to the database.
        /// </summary>
        public DBContext CurrentContext { get; }

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        public SqliteConnection? Connection { get; private set; }

        /// <summary>
        /// Checks if the connection to the database is ok.
        /// </summary>
        public bool IsConnected => Connection != null;

        /// <summary>
        /// Gets the connection exception if there was a connection error.
        /// </summary>
        public Exception? ConnectionException { get; }

        #region Tables

        /// <summary>
        /// List of all the tables.
        /// </summary>
        private readonly List<Table> _tables = new();

        /// <summary>
        /// Gets the table for managing the cards.
        /// </summary>
        public Cards Cards => (Cards)_tables[0];

        /// <summary>
        /// Gets the table for managing the categories.
        /// </summary>
        public Categories Categories => (Categories)_tables[1];

        /// <summary>
        /// Gets the table for managing the documents.
        /// </summary>
        public Documents Documents => (Documents)_tables[2];

        /// <summary>
        /// Gets the table for managing the notes.
        /// </summary>
        public Notes Notes => (Notes)_tables[3];

        /// <summary>
        /// Gets the table for managing the passwords.
        /// </summary>
        public Passwords Passwords => (Passwords)_tables[4];

        /// <summary>
        /// Gets the table for managing the reports.
        /// </summary>
        public Reports Reports => (Reports)_tables[5];

        /// <summary>
        /// Gets the table for managing the user settings.
        /// </summary>
        public UserSettings UserSettings => (UserSettings)_tables[6];

        /// <summary>
        /// Gets the table for managing the weak passwords.
        /// </summary>
        public WeakPasswords WeakPasswords => (WeakPasswords)_tables[7];

        #endregion

        #region Instance

        private static DB? _instance;

        /// <summary>
        /// Gets the instance of the database manager.<br/>
        /// If <see cref="Context"/> is changed, or the instance is not connected to the database,
        /// is created a new instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static DB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DB(Context);
                }
                else if (!_instance.IsConnected || !_instance.CurrentContext.Equals(Context))
                {
                    _instance.Dispose();
                    _instance = new DB(Context);
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DB"/> with the specified <see cref="DBContext"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private DB(DBContext? context)
        {
            //Checks if the information needed to connect to the database are present.
            if (context == null) throw new ArgumentNullException(nameof(context), "The database context cannot be null.");

            //Saves the connection info.
            CurrentContext = context;

            //Tries to open a connection to the database.
            //If fails, the instance is disposed.
            try
            {
                //Creates the directory of the database if not exists, then opens the connection.
                Directory.CreateDirectory(Path.GetDirectoryName(CurrentContext.DatabasePath) ?? string.Empty);
                Connection = new SqliteConnection(CurrentContext.ConnectionString);
                Connection.Open();

                //Initializes the database.
                InitializeTables();
                UpdateDatabase(Version, GetVersion());
            }
            catch (Exception e)
            {
                Dispose();
                ConnectionException = e;
            }
        }

        #endregion

        #region Initialization

        private void InitializeTables()
        {
            _tables.Add(new Cards(this));
            _tables.Add(new Categories(this));
            _tables.Add(new Documents(this));
            _tables.Add(new Notes(this));
            _tables.Add(new Passwords(this));
            _tables.Add(new Reports(this));
            _tables.Add(new UserSettings(this));
            _tables.Add(new WeakPasswords(this));
        }

        /// <summary>
        /// Updates the database from an old version to a new version.
        /// </summary>
        /// <exception cref="FileFormatException"></exception>
        private void UpdateDatabase(int newVersion, int oldVersion)
        {
            if (newVersion > oldVersion)
            {
                _tables.ForEach(table => table.Update(newVersion, oldVersion));
                SetVersion(newVersion);
            }
            else if (newVersion < oldVersion)
            {
                throw new FileFormatException("Is not possible to downgrade the database.");
            }
        }

        #region Version

        private int GetVersion()
        {
            UserSetting? version = UserSettings.Get("version");
            return version != null ? int.Parse(version.Value) : 0;
        }

        private void SetVersion(int version)
        {
            if (UserSettings.Exists("version")) UserSettings.Update(new("version", version.ToString() ?? "0"));
            else UserSettings.Add(new("version", version.ToString() ?? "0"));
        }

        #endregion

        #endregion

        /// <summary>
        /// Closes the connection and clears the instance.
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
            _tables.Clear();
        }

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        public static void DisposeInstance() => _instance?.Dispose();

        /// <summary>
        /// Changes the password of the database.
        /// </summary>
        /// <param name="newPassword">New password for the database.</param>
        public void ChangePassword(string newPassword)
        {
            if (Connection == null) return;

            //Injects the password securely into the query.
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT quote($newPassword);";
            command.Parameters.AddWithValue("$newPassword", newPassword);
            command.CommandText = "PRAGMA rekey = " + command.ExecuteScalar();
            command.Parameters.Clear();

            //Changes the password.
            command.ExecuteNonQuery();

            //Updates the context with the new password, then disposes the current instance.
            Context = new DBContext(CurrentContext.DatabasePath, newPassword);
            Dispose();
        }
    }
}
