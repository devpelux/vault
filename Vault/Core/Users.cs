using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public record User(int ID, string Username, string Password, string Key);

    public class Users : ITable
    {
        private readonly VaultDB VaultDB;

        public const int NewID = -1;


        public Users(VaultDB vaultDB) => VaultDB = vaultDB;

        public void CreateTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS Users (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "Username TEXT NOT NULL UNIQUE, " +
                                        "Password TEXT NOT NULL, " +
                                        "Key TEXT NOT NULL, " +
                                        "PRIMARY KEY(ID AUTOINCREMENT)" +
                                        ");";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void DeleteTable()
        {
            string command = "DROP TABLE IF EXISTS Users;";
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

        public void AddRecord(User record)
        {
            string command = "INSERT INTO Users (Username, Password, Key) " +
                                    "VALUES (@Username, @Password, @Key);";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Username", record.Username);
            query.Parameters.AddWithValue("@Password", record.Password);
            query.Parameters.AddWithValue("@Key", record.Key);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool RemoveRecord(int id)
        {
            try
            {
                string command = "DELETE FROM Users WHERE ID = @ID;";
                SqliteCommand query = new(command, VaultDB.Connection);
                query.Parameters.AddWithValue("@ID", id);
                query.Prepare();
                query.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException e)
            {
                return e.SqliteErrorCode == 19 ? false : throw e;
            }
        }

        public List<User> GetAllRecords()
        {
            List<User> records = new();
            string command = "SELECT * FROM Users";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public User GetRecord(int id)
        {
            string command = "SELECT * FROM Users WHERE ID = @ID;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        public User GetRecord(string username)
        {
            string command = "SELECT * FROM Users WHERE Username = @Username;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Username", username);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        public void UpdateRecord(User record)
        {
            string command = "UPDATE Users " +
                                    "SET Username = @Username, " +
                                        "Password = @Password, " +
                                        "Key = @Key, " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@Username", record.Username);
            query.Parameters.AddWithValue("@Password", record.Password);
            query.Parameters.AddWithValue("@Key", record.Key);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            string command = "SELECT COUNT(*) FROM Users WHERE ID = @ID;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public bool Exists(string username)
        {
            string command = "SELECT COUNT(*) FROM Users WHERE Username = @Username;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Username", username);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public int Count()
        {
            string command = "SELECT COUNT(*) FROM Users;";
            SqliteCommand query = new(command, VaultDB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        private static User ReadRecord(SqliteDataReader reader)
            => new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
    }
}
