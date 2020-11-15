using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vault.Core
{
    public record Category(int ID, int User, string Label, bool IsExpanded);
    public record CategoryValues(Category Category, IEnumerable Values, int Count);

    public class Categories : ITable
    {
        private readonly VaultDB VaultDB;

        public const int NewID = -1;
        public const int InvalidID = -1;


        public Categories(VaultDB vaultDB) => VaultDB = vaultDB;

        public void CreateTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS Categories (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "User INTEGER NOT NULL, " +
                                        "Label TEXT NOT NULL, " +
                                        "IsExpanded INTEGER NOT NULL, " +
                                        "PRIMARY KEY(ID AUTOINCREMENT), " +
                                        "FOREIGN KEY(User) REFERENCES Users(ID)" +
                                        ");";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void DeleteTable()
        {
            string command = "DROP TABLE IF EXISTS Categories;";
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

        public void AddRecord(Category record)
        {
            string command = "INSERT INTO Categories (User, Label, IsExpanded) " +
                                    "VALUES (@User, @Label, @IsExpanded);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Label", record.Label);
            query.Parameters.AddWithValue("@IsExpanded", record.IsExpanded ? 1 : 0);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void RemoveRecord(int id)
        {
            string command = "DELETE FROM Categories WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public List<Category> GetAllRecords()
        {
            List<Category> records = new();
            string command = "SELECT * FROM Categories";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public Category GetRecord(int id)
        {
            string command = "SELECT * FROM Categories WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        public List<Category> GetRecords(int user)
        {
            List<Category> records = new();
            string command = "SELECT * FROM Categories WHERE User = @User;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public List<Category> GetRecords(string label)
        {
            List<Category> records = new();
            string command = "SELECT * FROM Categories WHERE Label LIKE @Label;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Title", $"%{label}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public List<Category> GetRecords(string label, int user)
        {
            List<Category> records = new();
            string command = "SELECT * FROM Categories WHERE User = @User AND Label LIKE @Label;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Parameters.AddWithValue("@Label", $"%{label}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public void UpdateRecord(Category record)
        {
            string command = "UPDATE Categories " +
                                    "SET User = @User, " +
                                        "Label = @Label, " +
                                        "IsExpanded = @IsExpanded " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Label", record.Label);
            query.Parameters.AddWithValue("@IsExpanded", record.IsExpanded ? 1 : 0);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            string command = "SELECT COUNT(*) FROM Categories WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public int Count()
        {
            string command = "SELECT COUNT(*) FROM Categories;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        private static Category ReadRecord(SqliteDataReader reader)
            => new Category
            (
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3) == 1
            );
    }
}
