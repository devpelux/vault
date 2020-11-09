using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public class Passwords : ITable
    {
        private readonly VaultDB VaultDB;


        public Passwords(VaultDB vaultDB) => VaultDB = vaultDB;

        public void CreateTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS Passwords (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "UserID INTEGER NOT NULL, " +
                                        "Title TEXT NOT NULL, " +
                                        "Category TEXT NOT NULL, " +
                                        "Website TEXT NOT NULL, " +
                                        "Username TEXT NOT NULL, " +
                                        "Key TEXT NOT NULL, " +
                                        "Details TEXT NOT NULL, " +
                                        "Note TEXT NOT NULL, " +
                                        "RequestKey INTEGER NOT NULL, " +
                                        "PRIMARY KEY(ID AUTOINCREMENT), " +
                                        "FOREIGN KEY(UserID) REFERENCES Users(ID)" +
                                        ");";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void DeleteTable()
        {
            string command = "DROP TABLE IF EXISTS Passwords;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
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

        public void AddRecord(Password record)
        {
            string command = "INSERT INTO Passwords (UserID, Title, Category, Website, Username, Key, Details, Note, RequestKey) " +
                                    "VALUES (@UserID, @Title, @Category, @Website, @Username, @Key, @Details, @Note, @RequestKey);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Website", record.Website);
            query.Parameters.AddWithValue("@Username", record.Username);
            query.Parameters.AddWithValue("@Key", record.Key);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@Note", record.Note);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void RemoveRecord(int id)
        {
            string command = "DELETE FROM Passwords WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public List<Password> GetAllRecords()
        {
            List<Password> records = new List<Password>();
            string command = "SELECT * FROM Passwords";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public Password GetRecord(int id)
        {
            string command = "SELECT * FROM Passwords WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadElementFromReader(reader);
            return null;
        }

        public List<Password> GetRecords(int userId)
        {
            List<Password> records = new List<Password>();
            string command = "SELECT Passwords.ID, " +
                                    "Passwords.UserID, " +
                                    "Passwords.Title, " +
                                    "Passwords.Category, " +
                                    "Passwords.Website, " +
                                    "Passwords.Username, " +
                                    "Passwords.Key, " +
                                    "Passwords.Details, " +
                                    "Passwords.Note, " +
                                    "Passwords.RequestKey " +
                                    "FROM Passwords " +
                                    "JOIN Users ON Passwords.UserID = Users.ID " +
                                    "WHERE Users.ID = @UserID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", userId);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public List<Password> GetRecords(string title)
        {
            List<Password> records = new List<Password>();
            string command = "SELECT * FROM Passwords WHERE Title LIKE @Title;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Title", $"%{title}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public List<Password> GetRecords(string title, int userId)
        {
            List<Password> records = new List<Password>();
            string command = "SELECT Passwords.ID, " +
                                    "Passwords.UserID, " +
                                    "Passwords.Title, " +
                                    "Passwords.Category, " +
                                    "Passwords.Website, " +
                                    "Passwords.Username, " +
                                    "Passwords.Key, " +
                                    "Passwords.Details, " +
                                    "Passwords.Note, " +
                                    "Passwords.RequestKey " +
                                    "FROM Passwords " +
                                    "JOIN Users ON Passwords.UserID = Users.ID " +
                                    "WHERE Users.ID = @UserID AND Passwords.Title LIKE @Title;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", userId);
            query.Parameters.AddWithValue("@Title", $"%{title}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public void UpdateRecord(Password record)
        {
            string command = "UPDATE Passwords " +
                                    "SET UserID = @UserID, " +
                                        "Title = @Title, " +
                                        "Category = @Category, " +
                                        "Website = @Website, " +
                                        "Username = @Username, " +
                                        "Key = @Key, " +
                                        "Details = @Details, " +
                                        "Note = @Note, " +
                                        "RequestKey = @RequestKey " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Website", record.Website);
            query.Parameters.AddWithValue("@Username", record.Username);
            query.Parameters.AddWithValue("@Key", record.Key);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@Note", record.Note);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            string command = "SELECT COUNT(*) FROM Passwords WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public int Count()
        {
            string command = "SELECT COUNT(*) FROM Passwords;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        private static Password ReadElementFromReader(SqliteDataReader reader)
        {
            return new Password
            {
                ID = reader.GetInt32(0),
                UserID = reader.GetInt32(1),
                Title = reader.GetString(2),
                Category = reader.GetString(3),
                Website = reader.GetString(4),
                Username = reader.GetString(5),
                Key = reader.GetString(6),
                Details = reader.GetString(7),
                Note = reader.GetString(8),
                RequestKey = reader.GetInt32(9) == 1
            };
        }
    }
}
