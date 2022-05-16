using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Vault.Core.Database.Data;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the cards.
    /// </summary>
    public class Cards : Table
    {
        /// <inheritdoc/>
        public Cards(DB db) : base(db) { }

        /// <inheritdoc/>
        public override void Create()
        {
            string command =
                @"
                    CREATE TABLE IF NOT EXISTS `Cards` (
                    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
                    `category` TEXT NOT NULL REFERENCES `Categories` ( `name` ) ON DELETE RESTRICT ON UPDATE CASCADE,
                    `name` TEXT NOT NULL DEFAULT """",
                    `owner` TEXT NOT NULL DEFAULT """",
                    `number` TEXT NOT NULL DEFAULT """",
                    `type` TEXT NOT NULL DEFAULT """",
                    `cvv` TEXT NOT NULL DEFAULT """",
                    `iban` TEXT,
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
            string command = "DROP TABLE IF EXISTS `Cards`;";
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
        /// Adds a new card to the table.
        /// </summary>
        public void Add(Card card)
        {
            string command =
                @"
                    INSERT INTO `Cards` (`category`, `name`, `owner`, `number`, `type`, `cvv`, `iban`, `expiration`, `notes`, `locked`)
                    VALUES (@category, @name, @owner, @number, @type, @cvv, @iban, @expiration, @notes, @locked);
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@category", card.Category);
            query.Parameters.AddWithValue("@name", card.Name);
            query.Parameters.AddWithValue("@owner", card.Owner);
            query.Parameters.AddWithValue("@number", card.Number);
            query.Parameters.AddWithValue("@type", card.Type);
            query.Parameters.AddWithValue("@cvv", card.Cvv);
            query.Parameters.AddWithValue("@iban", card.Iban);
            query.Parameters.AddWithValue("@expiration", card.Expiration);
            query.Parameters.AddWithValue("@notes", card.Notes);
            query.Parameters.AddWithValue("@locked", card.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the card with the specified id.
        /// </summary>
        public void Remove(int id)
        {
            string command = "DELETE FROM `Cards` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all the cards.
        /// </summary>
        public List<Card> GetAll()
        {
            List<Card> cards = new();

            //Selects all the cards.
            string command = "SELECT * FROM `Cards`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the cards from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) cards.Add(ReadRecord(reader));

            return cards;
        }

        /// <summary>
        /// Gets the card with the specified id.
        /// If the card does not exists, returns null.
        /// </summary>
        public Card? Get(int id)
        {
            //Selects the card with the specified id.
            string command = "SELECT * FROM `Cards` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //Reads the card from the result, and, if there is no result, returns null.
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        /// <summary>
        /// Updates the specified card, if exists.
        /// </summary>
        public void Update(Card card)
        {
            string command =
                @"
                    UPDATE `Cards`
                    SET `category` = @category,
                        `name` = @name,
                        `owner` = @owner,
                        `number` = @number,
                        `type` = @type,
                        `cvv` = @cvv,
                        `iban` = @iban,
                        `expiration` = @expiration,
                        `notes` = @notes,
                        `locked` = @locked
                    WHERE `id` = @id;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@id", card.Id);
            query.Parameters.AddWithValue("@category", card.Category);
            query.Parameters.AddWithValue("@name", card.Name);
            query.Parameters.AddWithValue("@owner", card.Owner);
            query.Parameters.AddWithValue("@number", card.Number);
            query.Parameters.AddWithValue("@type", card.Type);
            query.Parameters.AddWithValue("@cvv", card.Cvv);
            query.Parameters.AddWithValue("@iban", card.Iban);
            query.Parameters.AddWithValue("@expiration", card.Expiration);
            query.Parameters.AddWithValue("@notes", card.Notes);
            query.Parameters.AddWithValue("@locked", card.IsLocked);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the card with the specified id exists.
        /// </summary>
        public bool Exists(int id)
        {
            //Counts the number of cards with the specified id.
            string command = "SELECT COUNT(*) FROM `Cards` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //If the card exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of cards.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `Cards`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Reads a card record from the reader.
        /// </summary>
        private static Card ReadRecord(SqliteDataReader reader)
            => new(reader.GetInt32(0),
                   reader.GetString(1),
                   reader.GetString(2),
                   reader.GetString(3),
                   reader.GetString(4),
                   reader.GetString(5),
                   reader.GetString(6),
                   reader.GetString(7),
                   reader.GetInt64(8),
                   reader.GetString(9),
                   reader.GetBoolean(10));
    }
}
