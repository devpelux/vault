using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Vault.Core.Database.Data;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the passwords.
    /// </summary>
    public class Passwords : Table
    {
        /// <inheritdoc/>
        public Passwords(DB db) : base(db) { }

        /// <inheritdoc/>
        public override void Create()
        {
            string command =
                @"
                    CREATE TABLE IF NOT EXISTS `Passwords` (
                    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
                    `category` TEXT NOT NULL REFERENCES `Categories` ( `name` ) ON DELETE RESTRICT ON UPDATE CASCADE,
                    `account` TEXT NOT NULL DEFAULT """",
                    `timestamp` INTEGER NOT NULL DEFAULT -1,
                    `username` TEXT,
                    `value` TEXT NOT NULL DEFAULT """",
                    `notes` TEXT,
                    `violated` INTEGER NOT NULL DEFAULT 0,
                    `locked` INTEGER NOT NULL DEFAULT 0
                    );
                ";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public override void Delete()
        {
            string command = "DROP TABLE IF EXISTS `Passwords`;";
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
        /// Adds a new password to the table.
        /// </summary>
        public void Add(Password password)
        {
            string command =
                @"
                    INSERT INTO `Passwords` (`category`, `account`, `timestamp`, `username`, `value`, `notes`, `violated`, `locked`)
                    VALUES (@category, @account, @timestamp, @username, @value, @notes, @violated, @locked);
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@category", password.Category);
            query.Parameters.AddWithValue("@account", password.Account);
            query.Parameters.AddWithValue("@timestamp", password.Timestamp);
            query.Parameters.AddWithValue("@username", password.Username);
            query.Parameters.AddWithValue("@value", password.Value);
            query.Parameters.AddWithValue("@notes", password.Notes);
            query.Parameters.AddWithValue("@violated", password.IsViolated);
            query.Parameters.AddWithValue("@locked", password.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the password with the specified id.
        /// </summary>
        public void Remove(int id)
        {
            string command = "DELETE FROM `Passwords` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all the passwords.
        /// </summary>
        public List<Password> GetAll()
        {
            List<Password> passwords = new();

            //Selects all the passwords.
            string command = "SELECT * FROM `Passwords`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the passwords from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) passwords.Add(ReadRecord(reader));

            return passwords;
        }

        /// <summary>
        /// Gets the password with the specified id.
        /// If the password does not exists, returns null.
        /// </summary>
        public Password? Get(int id)
        {
            //Selects the password with the specified id.
            string command = "SELECT * FROM `Passwords` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //Reads the password from the result, and, if there is no result, returns null.
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        /// <summary>
        /// Updates the specified password, if exists.
        /// </summary>
        public void Update(Password password)
        {
            string command =
                @"
                    UPDATE `Passwords`
                    SET `category` = @category,
                        `account` = @account,
                        `timestamp` = @timestamp,
                        `username` = @username,
                        `value` = @value,
                        `notes` = @notes,
                        `violated` = @violated,
                        `locked` = @locked
                    WHERE `id` = @id;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@id", password.Id);
            query.Parameters.AddWithValue("@category", password.Category);
            query.Parameters.AddWithValue("@account", password.Account);
            query.Parameters.AddWithValue("@timestamp", password.Timestamp);
            query.Parameters.AddWithValue("@username", password.Username);
            query.Parameters.AddWithValue("@value", password.Value);
            query.Parameters.AddWithValue("@notes", password.Notes);
            query.Parameters.AddWithValue("@violated", password.IsViolated);
            query.Parameters.AddWithValue("@locked", password.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the password with the specified id exists.
        /// </summary>
        public bool Exists(int id)
        {
            //Counts the number of passwords with the specified id.
            string command = "SELECT COUNT(*) FROM `Passwords` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //If the password exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of passwords.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `Passwords`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Counts the number of duplicated passwords.
        /// </summary>
        public int DuplicatedCount()
        {
            string command =
                @"
                    SELECT COUNT(DISTINCT(p.`value`))
                    FROM `Passwords` AS p
	                WHERE (
		                SELECT COUNT(*)
		                FROM `Passwords` AS p2
		                WHERE p2.`value` = p.`value` AND p2.`id` <> p.`id`
	                ) > 0;
                ";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Counts the number of weak passwords.
        /// </summary>
        public int WeakCount()
        {
            string command =
                @"
                    SELECT COUNT(*)
	                FROM `Passwords`
	                WHERE `value` IN ( SELECT * FROM `WeakPasswords` );
                ";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Counts the number of password that older than the time specified.
        /// </summary>
        public int OldCount(ulong time)
        {
            string command =
                @"
                    SELECT COUNT(*)
	                FROM `Passwords`
	                WHERE (@currentTimestamp - `timestamp`) > @time;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the time into the query.
            query.Parameters.AddWithValue("@currentTimestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            query.Parameters.AddWithValue("@time", (long)time);

            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Counts the number of violated passwords.
        /// </summary>
        public int ViolatedCount()
        {
            string command =
                @"
                    SELECT COUNT(*)
	                FROM `Passwords`
	                WHERE `violated` = 1;
                ";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Reads a password record from the reader.
        /// </summary>
        private static Password ReadRecord(SqliteDataReader reader)
            => new(reader.GetInt32(0),
                   reader.GetString(1),
                   reader.GetString(2),
                   reader.GetInt64(3),
                   reader.GetString(4),
                   reader.GetString(5),
                   reader.GetString(6),
                   reader.GetBoolean(7),
                   reader.GetBoolean(8));
    }
}
