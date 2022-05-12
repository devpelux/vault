using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Vault.Core.Database.Data;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the user settings.
    /// </summary>
    public class UserSettings : Table
    {
        /// <inheritdoc/>
        public UserSettings(DB db) : base(db) { }

        /// <inheritdoc/>
        public override void Create()
        {
            string command =
                @"
                    CREATE TABLE IF NOT EXISTS `UserSettings` (
                    `key` TEXT PRIMARY KEY,
                    `value` TEXT NOT NULL DEFAULT """"
                    );
                ";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();

            //Adds the "version" setting.
            Add(new("version", "0"));
        }

        /// <inheritdoc/>
        public override void Delete()
        {
            string command = "DROP TABLE IF EXISTS `UserSettings`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public override void Update(int newVersion, int oldVersion)
        {
            if (oldVersion == 0) Create();
        }

        /// <summary>
        /// Adds a new setting to the table.
        /// </summary>
        public void Add(UserSetting setting)
        {
            string command =
                @"
                    INSERT INTO `UserSettings` (`key`, `value`)
                    VALUES (@key, @value);
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@key", setting.Key);
            query.Parameters.AddWithValue("@value", setting.Value);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the setting with the specified key.
        /// </summary>
        public void Remove(string key)
        {
            string command = "DELETE FROM `UserSettings` WHERE `key` = @key;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the key into the query.
            query.Parameters.AddWithValue("@key", key);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all the settings.
        /// </summary>
        public List<UserSetting> GetAll()
        {
            List<UserSetting> settings = new();

            //Selects all the settings.
            string command = "SELECT * FROM `UserSettings`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the settings from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) settings.Add(ReadRecord(reader));

            return settings;
        }

        /// <summary>
        /// Gets the setting with the specified key.
        /// If the setting does not exists, returns null.
        /// </summary>
        public UserSetting? Get(string key)
        {
            if (!TableExists()) return null;

            //Selects the setting with the specified key.
            string command = "SELECT * FROM `UserSettings` WHERE `key` = @key;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the key into the query.
            query.Parameters.AddWithValue("@key", key);

            query.Prepare();

            //Reads the setting from the result, and, if there is no result, returns null.
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        /// <summary>
        /// Updates the specified setting, if exists.
        /// </summary>
        public void Update(UserSetting setting)
        {
            string command =
                @"
                    UPDATE `UserSettings`
                    SET `value` = @value
                    WHERE `key` = @key;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@key", setting.Key);
            query.Parameters.AddWithValue("@value", setting.Value);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the setting with the specified key exists.
        /// </summary>
        public bool Exists(string key)
        {
            //Counts the number of settings with the specified key.
            string command = "SELECT COUNT(*) FROM `UserSettings` WHERE `key` = @key;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the key into the query.
            query.Parameters.AddWithValue("@key", key);

            query.Prepare();

            //If the setting exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of settings.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `UserSettings`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Checks if the table exists.
        /// </summary>
        public bool TableExists()
        {
            string tableCkeck = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'UserSettings';";
            SqliteCommand tableCkeckQuery = new(tableCkeck, DB.Connection);
            tableCkeckQuery.Prepare();
            return Convert.ToInt32(tableCkeckQuery.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Reads a setting record from the reader.
        /// </summary>
        private static UserSetting ReadRecord(SqliteDataReader reader)
            => new(reader.GetString(0),
                   reader.GetString(1));
    }
}
