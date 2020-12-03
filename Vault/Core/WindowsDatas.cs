using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public record WindowsData(int ID, int User, int Window, double Height, double Width, double Top, double Left);

    public class WindowsDatas : ITable
    {
        private readonly VaultDB VaultDB;

        public const int NewID = -1;
        public const int InvalidID = -1;


        public WindowsDatas(VaultDB vaultDB) => VaultDB = vaultDB;

        public void CreateTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS WindowsDatas (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "User INTEGER NOT NULL, " +
                                        "Window INTEGER NOT NULL, " +
                                        "Height REAL NOT NULL, " +
                                        "Width REAL NOT NULL, " +
                                        "Top REAL NOT NULL, " +
                                        "Left REAL NOT NULL, " +
                                        "PRIMARY KEY(ID AUTOINCREMENT), " +
                                        "FOREIGN KEY(User) REFERENCES Users(ID)" +
                                        ");";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void DeleteTable()
        {
            string command = "DROP TABLE IF EXISTS WindowsDatas;";
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

        public void AddRecord(WindowsData record)
        {
            string command = "INSERT INTO WindowsDatas (User, Window, Height, Width, Top, Left) " +
                                    "VALUES (@User, @Window, @Height, @Width, @Top, @Left);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Window", record.Window);
            query.Parameters.AddWithValue("@Height", record.Height);
            query.Parameters.AddWithValue("@Width", record.Width);
            query.Parameters.AddWithValue("@Top", record.Top);
            query.Parameters.AddWithValue("@Left", record.Left);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool RemoveRecord(int id)
        {
            try
            {
                string command = "DELETE FROM WindowsDatas WHERE ID = @ID;";
                SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
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

        public List<WindowsData> GetAllRecords()
        {
            List<WindowsData> records = new();
            string command = "SELECT * FROM WindowsDatas";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public WindowsData GetRecord(int id)
        {
            string command = "SELECT * FROM WindowsDatas WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        public List<WindowsData> GetRecords(int user)
        {
            List<WindowsData> records = new();
            string command = "SELECT * FROM WindowsDatas WHERE User = @User;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public void UpdateRecord(WindowsData record)
        {
            string command = "UPDATE WindowsDatas " +
                                    "SET User = @User, " +
                                        "Window = @Window, " +
                                        "Height = @Height, " +
                                        "Width = @Width, " +
                                        "Top = @Top, " +
                                        "Left = @Left " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Window", record.Window);
            query.Parameters.AddWithValue("@Height", record.Height);
            query.Parameters.AddWithValue("@Width", record.Width);
            query.Parameters.AddWithValue("@Top", record.Top);
            query.Parameters.AddWithValue("@Left", record.Left);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            string command = "SELECT COUNT(*) FROM WindowsDatas WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public int Count()
        {
            string command = "SELECT COUNT(*) FROM WindowsDatas;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        private static WindowsData ReadRecord(SqliteDataReader reader)
            => new WindowsData
            (
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                reader.GetDouble(3),
                reader.GetDouble(4),
                reader.GetDouble(5),
                reader.GetDouble(6)
            );
    }
}
