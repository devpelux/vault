using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Vault.Core.Database.Data;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the documents.
    /// </summary>
    public class Documents : Table
    {
        /// <inheritdoc/>
        public Documents(DB db) : base(db) { }

        /// <inheritdoc/>
        public override void Create()
        {
            string command =
                @"
                    CREATE TABLE IF NOT EXISTS `Documents` (
                    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
                    `category` TEXT NOT NULL REFERENCES `Categories` ( `name` ) ON DELETE RESTRICT ON UPDATE CASCADE,
                    `name` TEXT NOT NULL DEFAULT """",
                    `owner` TEXT,
                    `code` TEXT,
                    `expiration` INTEGER NOT NULL DEFAULT -1,
                    `notes` TEXT,
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
            string command = "DROP TABLE IF EXISTS `Documents`;";
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
        /// Adds a new document to the table.
        /// </summary>
        public void Add(Document document)
        {
            string command =
                @"
                    INSERT INTO `Documents` (`category`, `name`, `owner`, `code`, `expiration`, `notes`, `locked`)
                    VALUES (@category, @name, @owner, @code, @expiration, @notes, @locked);
                ;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@category", document.Category);
            query.Parameters.AddWithValue("@name", document.Name);
            query.Parameters.AddWithValue("@owner", document.Owner);
            query.Parameters.AddWithValue("@code", document.Code);
            query.Parameters.AddWithValue("@expiration", document.Expiration);
            query.Parameters.AddWithValue("@notes", document.Notes);
            query.Parameters.AddWithValue("@locked", document.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the document with the specified id.
        /// </summary>
        public void Remove(int id)
        {
            string command = "DELETE FROM `Documents` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all the documents.
        /// </summary>
        public List<Document> GetAll()
        {
            List<Document> documents = new();

            //Selects all the documents.
            string command = "SELECT * FROM `Documents`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the documents from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) documents.Add(ReadRecord(reader));
            return documents;
        }

        /// <summary>
        /// Gets the document with the specified id.
        /// If the document does not exists, returns null.
        /// </summary>
        public Document? Get(int id)
        {
            //Selects the document with the specified id.
            string command = "SELECT * FROM `Documents` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //Reads the document from the result, and, if there is no result, returns null.
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        /// <summary>
        /// Updates the specified document, if exists.
        /// </summary>
        public void Update(Document document)
        {
            string command =
                @"
                    UPDATE `Documents`
                    SET `category` = @category,
                        `name` = @name,
                        `owner` = @owner,
                        `code` = @code,
                        `expiration` = @expiration,
                        `notes` = @notes,
                        `locked` = @locked
                    WHERE `id` = @id;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@id", document.Id);
            query.Parameters.AddWithValue("@category", document.Category);
            query.Parameters.AddWithValue("@name", document.Name);
            query.Parameters.AddWithValue("@owner", document.Owner);
            query.Parameters.AddWithValue("@code", document.Code);
            query.Parameters.AddWithValue("@expiration", document.Expiration);
            query.Parameters.AddWithValue("@notes", document.Notes);
            query.Parameters.AddWithValue("@locked", document.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the document with the specified id exists.
        /// </summary>
        public bool Exists(int id)
        {
            //Counts the number of documents with the specified id.
            string command = "SELECT COUNT(*) FROM `Documents` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //If the document exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of documents.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `Documents`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Reads a document record from the reader.
        /// </summary>
        private static Document ReadRecord(SqliteDataReader reader)
            => new(reader.GetInt32(0),
                   reader.GetString(1),
                   reader.GetString(2),
                   reader.GetString(3),
                   reader.GetString(4),
                   reader.GetInt64(5),
                   reader.GetString(6),
                   reader.GetBoolean(7));
    }
}
