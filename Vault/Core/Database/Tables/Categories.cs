using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Vault.Core.Database.Data;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the categories.
    /// </summary>
    public class Categories : Table
    {
        /// <inheritdoc/>
        public override void Create()
        {
            string command =
                @"
                    CREATE TABLE IF NOT EXISTS `Categories` (
                    `name` TEXT PRIMARY KEY,
                    `expanded` INTEGER NOT NULL DEFAULT 0
                    );
                ";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();

            //Adds the default category.
            Add(new("None", false));
        }

        /// <inheritdoc/>
        public override void Delete()
        {
            string command = "DROP TABLE IF EXISTS `Categories`;";
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
        /// Adds a new category to the table.
        /// </summary>
        public void Add(Category category)
        {
            string command =
                @"
                    INSERT INTO `Categories` (`name`, `expanded`)
                    VALUES (@name, @expanded);
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@name", category.Name);
            query.Parameters.AddWithValue("@expanded", category.IsExpanded);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the category with the specified id.
        /// If the category cannot be removed, because is used, returns false.
        /// </summary>
        public bool Remove(string name)
        {
            try
            {
                string command = "DELETE FROM `Categories` WHERE `name` = @name;";
                SqliteCommand query = new(command, DB.Connection);

                //Injects the name into the query.
                query.Parameters.AddWithValue("@name", name);

                query.Prepare();
                query.ExecuteNonQuery();

                return true;
            }
            catch (SqliteException e)
            {
                //If the category is used to categorize data, is not eliminable.
                //So returns false and does nothing.
                return e.SqliteErrorCode == 19 ? false : throw e;
            }
        }

        /// <summary>
        /// Gets all the categories.
        /// </summary>
        public List<Category> GetAll()
        {
            List<Category> categories = new();

            //Selects all the categories.
            string command = "SELECT * FROM `Categories`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the categories from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) categories.Add(ReadRecord(reader));

            return categories;
        }

        /// <summary>
        /// Gets the category with the specified name.
        /// If the category does not exists, returns null.
        /// </summary>
        public Category? Get(string name)
        {
            //Selects the category with the specified name.
            string command = "SELECT * FROM `Categories` WHERE `name` = @name;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the name into the query.
            query.Parameters.AddWithValue("@name", name);

            query.Prepare();

            //Reads the category from the result, and, if there is no result, returns null.
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        /// <summary>
        /// Updates the specified category, if exists.
        /// </summary>
        public void UpdateRecord(Category category)
        {
            string command =
                @"
                    UPDATE `Categories`
                    SET `expanded` = @expanded
                    WHERE `name` = @name;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@name", category.Name);
            query.Parameters.AddWithValue("@expanded", category.IsExpanded);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the category with the specified name exists.
        /// </summary>
        public bool Exists(string name)
        {
            //Counts the number of categories with the specified name.
            string command = "SELECT COUNT(*) FROM `Categories` WHERE `name` = @name;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the name into the query.
            query.Parameters.AddWithValue("@name", name);

            query.Prepare();

            //If the category exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of category.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `Categories`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Reads a category record from the reader.
        /// </summary>
        private static Category ReadRecord(SqliteDataReader reader)
            => new(reader.GetString(0),
                   reader.GetBoolean(1));
    }
}
