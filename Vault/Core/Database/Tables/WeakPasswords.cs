using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the weak passwords.
    /// </summary>
    public class WeakPasswords : Table
    {
        /// <inheritdoc/>
        public WeakPasswords(DB db) : base(db) { }

        /// <inheritdoc/>
        public override void Create()
        {
            string command = @"CREATE TABLE IF NOT EXISTS `WeakPasswords` ( `value` TEXT PRIMARY KEY );";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public override void Delete()
        {
            string command = "DROP TABLE IF EXISTS `WeakPasswords`;";
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
        /// Adds a new password to the table, if not exists.
        /// </summary>
        public void Add(string password)
        {
            string command =
                @"
                    INSERT INTO `WeakPasswords` (`value`)
                    VALUES (@value);
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the password into the query.
            query.Parameters.AddWithValue("@value", password);

            query.Prepare();

            try
            {
                query.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                return;
            }
        }

        /// <summary>
        /// Removes the specified password.
        /// </summary>
        public void Remove(string password)
        {
            string command = "DELETE FROM `WeakPasswords` WHERE `value` = @value;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the password into the query.
            query.Parameters.AddWithValue("@value", password);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes all the passwords.
        /// </summary>
        public void Clear()
        {
            string command = "DELETE FROM `WeakPasswords`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all the passwords.
        /// </summary>
        public List<string> GetAll()
        {
            List<string> passwords = new();

            //Selects all the passwords.
            string command = "SELECT * FROM `WeakPasswords`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the passwords from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) passwords.Add(ReadRecord(reader));

            return passwords;
        }

        /// <summary>
        /// Checks if the specified password exists.
        /// </summary>
        public bool Exists(string password)
        {
            //Counts the number of passwords equals to the specified password.
            string command = "SELECT COUNT(*) FROM `WeakPasswords` WHERE `value` = @value;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the password into the query.
            query.Parameters.AddWithValue("@value", password);

            query.Prepare();

            //If the password exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of passwords.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `WeakPasswords`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Reads a password from the reader.
        /// </summary>
        private static string ReadRecord(SqliteDataReader reader)
            => reader.GetString(0);
    }
}
