using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public class Notes : ITable
    {
        private readonly VaultDB VaultDB;

        public const int NewID = -1;
        public const int InvalidID = -1;


        public Notes(VaultDB vaultDB) => VaultDB = vaultDB;

        public void CreateTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS Notes (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "User INTEGER NOT NULL, " +
                                        "Category INTEGER NOT NULL, " +
                                        "RequestKey INTEGER NOT NULL, " +
                                        "Title TEXT NOT NULL, " +
                                        "Subtitle TEXT NOT NULL, " +
                                        "Text TEXT NOT NULL, " +
                                        "PRIMARY KEY(ID AUTOINCREMENT), " +
                                        "FOREIGN KEY(User) REFERENCES Users(ID), " +
                                        "FOREIGN KEY(Category) REFERENCES Categories(ID)" +
                                        ");";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void DeleteTable()
        {
            string command = "DROP TABLE IF EXISTS Notes;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void UpdateTable(int newVersion, int oldVersion)
        {
            if (oldVersion == 0)
            {
                CreateTable();
            }
        }

        public void AddRecord(Note record)
        {
            string command = "INSERT INTO Notes (User, Title, Category, Subtitle, RequestKey, Text) " +
                                    "VALUES (@User, @Title, @Category, @Subtitle, @RequestKey, @Text);";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Subtitle", record.Subtitle);
            query.Parameters.AddWithValue("@Text", record.Text);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void RemoveRecord(int id)
        {
            string command = "DELETE FROM Notes WHERE ID = @ID;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public List<Note> GetAllRecords()
        {
            List<Note> records = new();
            string command = "SELECT * FROM Notes";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public Note GetRecord(int id)
        {
            string command = "SELECT * FROM Notes WHERE ID = @ID;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        public List<Note> GetRecords(int user)
        {
            List<Note> records = new();
            string command = "SELECT * FROM Notes WHERE User = @User;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public void UpdateRecord(Note record)
        {
            string command = "UPDATE Notes " +
                                    "SET User = @User, " +
                                        "Category = @Category, " +
                                        "RequestKey = @RequestKey, " +
                                        "Title = @Title, " +
                                        "Subtitle = @Subtitle, " +
                                        "Text = @Text " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Subtitle", record.Subtitle);
            query.Parameters.AddWithValue("@Text", record.Text);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            string command = "SELECT COUNT(*) FROM Notes WHERE ID = @ID;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public int Count()
        {
            string command = "SELECT COUNT(*) FROM Notes;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        private static Note ReadRecord(SqliteDataReader reader)
            => new(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3) == 1, reader.GetString(4), reader.GetString(5), reader.GetString(6));
    }
}
