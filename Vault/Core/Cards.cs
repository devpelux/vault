using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public class Cards : ITable
    {
        private readonly VaultDB VaultDB;

        public const int NewID = -1;
        public const int InvalidID = -1;


        public Cards(VaultDB vaultDB) => VaultDB = vaultDB;

        public void CreateTable()
        {
            string command = "CREATE TABLE IF NOT EXISTS Cards (" +
                                        "ID INTEGER NOT NULL UNIQUE, " +
                                        "User INTEGER NOT NULL, " +
                                        "Category INTEGER NOT NULL, " +
                                        "RequestKey INTEGER NOT NULL, " +
                                        "Label TEXT NOT NULL, " +
                                        "Description TEXT NOT NULL, " +
                                        "Owner TEXT NOT NULL, " +
                                        "Type TEXT NOT NULL, " +
                                        "Number TEXT NOT NULL, " +
                                        "SecureCode TEXT NOT NULL, " +
                                        "Expiration TEXT NOT NULL, " +
                                        "Note TEXT NOT NULL, " +
                                        "PRIMARY KEY(ID AUTOINCREMENT), " +
                                        "FOREIGN KEY(User) REFERENCES Users(ID), " +
                                        "FOREIGN KEY(Category) REFERENCES Categories(ID)" +
                                        ");";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void DeleteTable()
        {
            string command = "DROP TABLE IF EXISTS Cards;";
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

        public void AddRecord(Card record)
        {
            string command = "INSERT INTO Cards (User, Category, RequestKey, Label, Description, Owner, Type, Number, SecureCode, Expiration, Note) " +
                                    "VALUES (@User, @Category, @RequestKey, @Label, @Description, @Owner, @Type, @Number, @SecureCode, @Expiration, @Note);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Label", record.Label);
            query.Parameters.AddWithValue("@Description", record.Description);
            query.Parameters.AddWithValue("@Owner", record.Owner);
            query.Parameters.AddWithValue("@Type", record.Type);
            query.Parameters.AddWithValue("@Number", record.Number);
            query.Parameters.AddWithValue("@SecureCode", record.SecureCode);
            query.Parameters.AddWithValue("@Expiration", record.Expiration);
            query.Parameters.AddWithValue("@Note", record.Note);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void RemoveRecord(int id)
        {
            string command = "DELETE FROM Cards WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public List<Card> GetAllRecords()
        {
            List<Card> records = new();
            string command = "SELECT * FROM Cards";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public Card GetRecord(int id)
        {
            string command = "SELECT * FROM Cards WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        public List<Card> GetRecords(int user)
        {
            List<Card> records = new();
            string command = "SELECT * FROM Cards WHERE User = @User;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public void UpdateRecord(Card record)
        {
            string command = "UPDATE Cards " +
                                    "SET User = @User, " +
                                        "Category = @Category, " +
                                        "RequestKey = @RequestKey, " +
                                        "Label = @Label, " +
                                        "Description = @Description, " +
                                        "Owner = @Owner, " +
                                        "Type = @Type, " +
                                        "Number = @Number, " +
                                        "SecureCode = @SecureCode, " +
                                        "Expiration = @Expiration, " +
                                        "Note = @Note " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Label", record.Label);
            query.Parameters.AddWithValue("@Description", record.Description);
            query.Parameters.AddWithValue("@Owner", record.Owner);
            query.Parameters.AddWithValue("@Type", record.Type);
            query.Parameters.AddWithValue("@Number", record.Number);
            query.Parameters.AddWithValue("@SecureCode", record.SecureCode);
            query.Parameters.AddWithValue("@Expiration", record.Expiration);
            query.Parameters.AddWithValue("@Note", record.Note);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            string command = "SELECT COUNT(*) FROM Cards WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public int Count()
        {
            string command = "SELECT COUNT(*) FROM Cards;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        private static Card ReadRecord(SqliteDataReader reader)
            => new Card
            (
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                reader.GetInt32(3) == 1,
                reader.GetString(4),
                reader.GetString(5),
                reader.GetString(6),
                reader.GetString(7),
                reader.GetString(8),
                reader.GetString(9),
                reader.GetString(10),
                reader.GetString(11)
            );
    }
}
