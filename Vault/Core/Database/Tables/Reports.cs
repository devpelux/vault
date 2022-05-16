using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Vault.Core.Database.Data;

namespace Vault.Core.Database.Tables
{
    /// <summary>
    /// Table for managing the reports.
    /// </summary>
    public class Reports : Table
    {
        /// <inheritdoc/>
        public Reports(DB db) : base(db) { }

        /// <inheritdoc/>
        public override void Create()
        {
            string command =
                @"
                    CREATE TABLE IF NOT EXISTS `Reports` (
                    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
                    `total` INTEGER NOT NULL DEFAULT 0,
                    `duplicated` INTEGER NOT NULL DEFAULT 0,
                    `weak` INTEGER NOT NULL DEFAULT 0,
                    `old` INTEGER NOT NULL DEFAULT 0,
                    `violated` INTEGER NOT NULL DEFAULT 0,
                    `timestamp` INTEGER NOT NULL DEFAULT -1
                    );
                ";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public override void Delete()
        {
            string command = "DROP TABLE IF EXISTS `Reports`;";
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
        /// Adds a new report to the table.
        /// </summary>
        public void Add(Report report)
        {
            string command =
                @"
                    INSERT INTO `Reports` (`total`, `duplicated`, `weak`, `old`, `violated`, `timestamp`)
                    VALUES (@total, @duplicated, @weak, @old, @violated, @timestamp);
                ;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@total", report.Total);
            query.Parameters.AddWithValue("@duplicated", report.Duplicated);
            query.Parameters.AddWithValue("@weak", report.Weak);
            query.Parameters.AddWithValue("@old", report.Old);
            query.Parameters.AddWithValue("@violated", report.Violated);
            query.Parameters.AddWithValue("@timestamp", report.Timestamp);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the report with the specified id.
        /// </summary>
        public void Remove(int id)
        {
            string command = "DELETE FROM `Reports` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets all the reports.
        /// </summary>
        public List<Report> GetAll()
        {
            List<Report> reports = new();

            //Selects all the reports.
            string command = "SELECT * FROM `Reports`";
            SqliteCommand query = new(command, DB.Connection);

            query.Prepare();

            //Reads all the reports from the result.
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) reports.Add(ReadRecord(reader));
            return reports;
        }

        /// <summary>
        /// Gets the report with the specified id.
        /// If the report does not exists, returns null.
        /// </summary>
        public Report? Get(int id)
        {
            //Selects the report with the specified id.
            string command = "SELECT * FROM `Reports` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //Reads the report from the result, and, if there is no result, returns null.
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        /// <summary>
        /// Updates the specified report, if exists.
        /// </summary>
        public void Update(Report report)
        {
            string command =
                @"
                    UPDATE `Reports`
                    SET `total` = @total,
                        `duplicated` = @duplicated,
                        `weak` = @weak,
                        `old` = @old,
                        `violated` = @violated,
                        `timestamp` = @timestamp
                    WHERE `id` = @id;
                ";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the record values into the query.
            query.Parameters.AddWithValue("@id", report.Id);
            query.Parameters.AddWithValue("@total", report.Total);
            query.Parameters.AddWithValue("@duplicated", report.Duplicated);
            query.Parameters.AddWithValue("@weak", report.Weak);
            query.Parameters.AddWithValue("@old", report.Old);
            query.Parameters.AddWithValue("@violated", report.Violated);
            query.Parameters.AddWithValue("@timestamp", report.Timestamp);

            query.Prepare();
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if the report with the specified id exists.
        /// </summary>
        public bool Exists(int id)
        {
            //Counts the number of reports with the specified id.
            string command = "SELECT COUNT(*) FROM `Reports` WHERE `id` = @id;";
            SqliteCommand query = new(command, DB.Connection);

            //Injects the id into the query.
            query.Parameters.AddWithValue("@id", id);

            query.Prepare();

            //If the report exists the result of the query must be 1.
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Counts the number of reports.
        /// </summary>
        public int Count()
        {
            string command = "SELECT COUNT(*) FROM `Reports`;";
            SqliteCommand query = new(command, DB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        /// <summary>
        /// Reads a report record from the reader.
        /// </summary>
        private static Report ReadRecord(SqliteDataReader reader)
            => new(reader.GetInt32(0),
                   reader.GetInt32(1),
                   reader.GetInt32(2),
                   reader.GetInt32(3),
                   reader.GetInt32(4),
                   reader.GetInt32(5),
                   reader.GetInt64(6));
    }
}
