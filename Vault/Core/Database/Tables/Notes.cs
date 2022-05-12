using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Vault.Core.Database.Data;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the notes.
    /// </summary>
    public class Notes : Table
    {
        /// <inheritdoc/>
        public Notes(DB db) : base(db) { }

        /// <inheritdoc/>
        public override void Create()
        {
            string command =
                @"
                    CREATE TABLE IF NOT EXISTS `Notes` (
                    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
                    `category` TEXT NOT NULL REFERENCES `Categories` ( `name` ) ON DELETE RESTRICT ON UPDATE CASCADE,
                    `title` TEXT NOT NULL DEFAULT """",
                    `text` TEXT NOT NULL DEFAULT """",
                    `timestamp` INTEGER NOT NULL DEFAULT -1,
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
            string command = "DROP TABLE IF EXISTS `Notes`;";
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
        /// Adds a new note to the table.
        /// </summary>
        public void Add(Note note)
        {
            string command =
                @"
                    INSERT INTO `Notes` (`category`, `title`, `text`, `timestamp`, `locked`)
                    VALUES (@category, @title, @text, @timestamp, @locked);
                ;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@category", note.Category);
            query.Parameters.AddWithValue("@title", note.Title);
            query.Parameters.AddWithValue("@text", note.Text);
            query.Parameters.AddWithValue("@timestamp", note.Timestamp);
            query.Parameters.AddWithValue("@locked", note.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the note with the specified id.
        /// </summary>
        public void Remove(int id)
        {
            string command = "DELETE FROM `Notes` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all the notes.
        /// </summary>
        public List<Note> GetAll()
        {
            List<Note> notes = new();

            //Selects all the notes.
            string command = "SELECT * FROM `Notes`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the notes from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) notes.Add(ReadRecord(reader));
            return notes;
        }

        /// <summary>
        /// Gets the note with the specified id.
        /// If the note does not exists, returns null.
        /// </summary>
        public Note? Get(int id)
        {
            //Selects the note with the specified id.
            string command = "SELECT * FROM `Notes` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //Reads the note from the result, and, if there is no result, returns null.
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        /// <summary>
        /// Updates the specified note, if exists.
        /// </summary>
        public void Update(Note note)
        {
            string command =
                @"
                    UPDATE `Notes`
                    SET `category` = @category,
                        `title` = @title,
                        `text` = @text,
                        `timestamp` = @timestamp,
                        `locked` = @locked
                    WHERE `id` = @id;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@id", note.Id);
            query.Parameters.AddWithValue("@category", note.Category);
            query.Parameters.AddWithValue("@title", note.Title);
            query.Parameters.AddWithValue("@text", note.Text);
            query.Parameters.AddWithValue("@timestamp", note.Timestamp);
            query.Parameters.AddWithValue("@locked", note.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the note with the specified id exists.
        /// </summary>
        public bool Exists(int id)
        {
            //Counts the number of notes with the specified id.
            string command = "SELECT COUNT(*) FROM `Notes` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //If the note exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of notes.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `Notes`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Reads a note record from the reader.
        /// </summary>
        private static Note ReadRecord(SqliteDataReader reader)
            => new(reader.GetInt32(0),
                   reader.GetString(1),
                   reader.GetString(2),
                   reader.GetString(3),
                   reader.GetInt64(4),
                   reader.GetBoolean(5));
    }
}
