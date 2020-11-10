﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public class Passwords : ITable
    {
        private readonly VaultDB VaultDB;

        public const int NewID = -1;
        public const int InvalidID = -1;


        public Passwords(VaultDB vaultDB) => VaultDB = vaultDB;

        public void CreateTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS Passwords (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "UserID INTEGER NOT NULL, " +
                                        "Title TEXT NOT NULL, " +
                                        "Category TEXT NOT NULL, " +
                                        "Details TEXT NOT NULL, " +
                                        "RequestKey INTEGER NOT NULL, " +
                                        "Website TEXT NOT NULL, " +
                                        "Username TEXT NOT NULL, " +
                                        "Key TEXT NOT NULL, " +
                                        "Note TEXT NOT NULL, " +
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
            string command = "INSERT INTO Passwords (UserID, Title, Category, Details, RequestKey, Website, Username, Key, Note) " +
                                    "VALUES (@UserID, @Title, @Category, @Details, @RequestKey, @Website, @Username, @Key, @Note);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Website", record.Website);
            query.Parameters.AddWithValue("@Username", record.Username);
            query.Parameters.AddWithValue("@Key", record.Key);
            query.Parameters.AddWithValue("@Note", record.Note);
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
                                    "Passwords.Details, " +
                                    "Passwords.RequestKey, " +
                                    "Passwords.Website, " +
                                    "Passwords.Username, " +
                                    "Passwords.Key, " +
                                    "Passwords.Note " +
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
                                    "Passwords.Details, " +
                                    "Passwords.RequestKey, " +
                                    "Passwords.Website, " +
                                    "Passwords.Username, " +
                                    "Passwords.Key, " +
                                    "Passwords.Note " +
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
                                        "Details = @Details, " +
                                        "RequestKey = @RequestKey, " +
                                        "Website = @Website, " +
                                        "Username = @Username, " +
                                        "Key = @Key, " +
                                        "Note = @Note " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Website", record.Website);
            query.Parameters.AddWithValue("@Username", record.Username);
            query.Parameters.AddWithValue("@Key", record.Key);
            query.Parameters.AddWithValue("@Note", record.Note);
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

        private Password ReadElementFromReader(SqliteDataReader reader)
        {
            return new Password
            {
                ID = reader.GetInt32(0),
                UserID = reader.GetInt32(1),
                Title = reader.GetString(2),
                Category = reader.GetString(3),
                Details = reader.GetString(4),
                RequestKey = reader.GetInt32(5) == 1,
                Website = reader.GetString(6),
                Username = reader.GetString(7),
                Key = reader.GetString(8),
                Note = reader.GetString(9)
            };
        }

        public static Password Encrypt(Password password, byte[] key)
        {
            if (password != null)
            {
                return new Password
                {
                    ID = password.ID,
                    UserID = password.UserID,
                    Title = password.Title,
                    Category = password.Category,
                    Details = password.Details,
                    RequestKey = password.RequestKey,
                    Website = password.Website,
                    Username = Encryptor.Encrypt(password.Username, key),
                    Key = Encryptor.Encrypt(password.Key, key),
                    Note = Encryptor.Encrypt(password.Note, key)
                };
            }
            return null;
        }

        public static Password Decrypt(Password encryptedPassword, byte[] key)
        {
            if (encryptedPassword != null)
            {
                return new Password
                {
                    ID = encryptedPassword.ID,
                    UserID = encryptedPassword.UserID,
                    Title = encryptedPassword.Title,
                    Category = encryptedPassword.Category,
                    Details = encryptedPassword.Details,
                    RequestKey = encryptedPassword.RequestKey,
                    Website = encryptedPassword.Website,
                    Username = Encryptor.Decrypt(encryptedPassword.Username, key),
                    Key = Encryptor.Decrypt(encryptedPassword.Key, key),
                    Note = Encryptor.Decrypt(encryptedPassword.Note, key)
                };
            }
            return null;
        }
    }
}
