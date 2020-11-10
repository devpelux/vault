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
                                        "UserID INTEGER NOT NULL, " +
                                        "Title TEXT NOT NULL, " +
                                        "Category TEXT NOT NULL, " +
                                        "Details TEXT NOT NULL, " +
                                        "RequestKey INTEGER NOT NULL, " +
                                        "Name TEXT NOT NULL, " +
                                        "Type TEXT NOT NULL, " +
                                        "Number TEXT NOT NULL, " +
                                        "SecureCode TEXT NOT NULL, " +
                                        "Expiration TEXT NOT NULL, " +
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
            string command = "INSERT INTO Cards (UserID, Title, Category, Details, RequestKey, Name, Type, Number, SecureCode, Expiration, Note) " +
                                    "VALUES (@UserID, @Title, @Category, @Details, @RequestKey, @Name, @Type, @Number, @SecureCode, @Expiration, @Note);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Name", record.Name);
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
            List<Card> records = new List<Card>();
            string command = "SELECT * FROM Cards";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public Card GetRecord(int id)
        {
            string command = "SELECT * FROM Cards WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadElementFromReader(reader);
            return null;
        }

        public List<Card> GetRecords(int userId)
        {
            List<Card> records = new List<Card>();
            string command = "SELECT Cards.ID, " +
                                    "Cards.UserID, " +
                                    "Cards.Title, " +
                                    "Cards.Category, " +
                                    "Cards.Details, " +
                                    "Cards.RequestKey, " +
                                    "Cards.Name, " +
                                    "Cards.Type, " +
                                    "Cards.Number, " +
                                    "Cards.SecureCode, " +
                                    "Cards.Expiration, " +
                                    "Cards.Note " +
                                    "FROM Cards " +
                                    "JOIN Users ON Cards.UserID = Users.ID " +
                                    "WHERE Users.ID = @UserID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", userId);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public List<Card> GetRecords(string title)
        {
            List<Card> records = new List<Card>();
            string command = "SELECT * FROM Cards WHERE Title LIKE @Title;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Title", $"%{title}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public List<Card> GetRecords(string title, int userId)
        {
            List<Card> records = new List<Card>();
            string command = "SELECT Cards.ID, " +
                                    "Cards.UserID, " +
                                    "Cards.Title, " +
                                    "Cards.Category, " +
                                    "Cards.Details, " +
                                    "Cards.RequestKey, " +
                                    "Cards.Name, " +
                                    "Cards.Type, " +
                                    "Cards.Number, " +
                                    "Cards.SecureCode, " +
                                    "Cards.Expiration, " +
                                    "Cards.Note " +
                                    "FROM Cards " +
                                    "JOIN Users ON Cards.UserID = Users.ID " +
                                    "WHERE Users.ID = @UserID AND Cards.Title LIKE @Title;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", userId);
            query.Parameters.AddWithValue("@Title", $"%{title}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public void UpdateRecord(Card record)
        {
            string command = "UPDATE Cards " +
                                    "SET UserID = @UserID, " +
                                        "Title = @Title, " +
                                        "Category = @Category, " +
                                        "Details = @Details, " +
                                        "RequestKey = @RequestKey, " +
                                        "Name = @Name, " +
                                        "Type = @Type, " +
                                        "Number = @Number, " +
                                        "SecureCode = @SecureCode, " +
                                        "Expiration = @Expiration, " +
                                        "Note = @Note " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Name", record.Name);
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

        private Card ReadElementFromReader(SqliteDataReader reader)
        {
            return new Card
            {
                ID = reader.GetInt32(0),
                UserID = reader.GetInt32(1),
                Title = reader.GetString(2),
                Category = reader.GetString(3),
                Details = reader.GetString(4),
                RequestKey = reader.GetInt32(5) == 1,
                Name = reader.GetString(6),
                Type = reader.GetString(7),
                Number = reader.GetString(8),
                SecureCode = reader.GetString(9),
                Expiration = reader.GetString(10),
                Note = reader.GetString(11)
            };
        }

        public static Card Encrypt(Card card, byte[] key)
        {
            if (card != null)
            {
                return new Card
                {
                    ID = card.ID,
                    UserID = card.UserID,
                    Title = card.Title,
                    Category = card.Category,
                    Details = card.Details,
                    RequestKey = card.RequestKey,
                    Name = Encryptor.Encrypt(card.Name, key),
                    Type = Encryptor.Encrypt(card.Type, key),
                    Number = Encryptor.Encrypt(card.Number, key),
                    SecureCode = Encryptor.Encrypt(card.SecureCode, key),
                    Expiration = Encryptor.Encrypt(card.Expiration, key),
                    Note = Encryptor.Encrypt(card.Note, key)
                };
            }
            return null;
        }

        public static Card Decrypt(Card encryptedCard, byte[] key)
        {
            if (encryptedCard != null)
            {
                return new Card
                {
                    ID = encryptedCard.ID,
                    UserID = encryptedCard.UserID,
                    Title = encryptedCard.Title,
                    Category = encryptedCard.Category,
                    Details = encryptedCard.Details,
                    RequestKey = encryptedCard.RequestKey,
                    Name = Encryptor.Decrypt(encryptedCard.Name, key),
                    Type = Encryptor.Decrypt(encryptedCard.Type, key),
                    Number = Encryptor.Decrypt(encryptedCard.Number, key),
                    SecureCode = Encryptor.Decrypt(encryptedCard.SecureCode, key),
                    Expiration = Encryptor.Decrypt(encryptedCard.Expiration, key),
                    Note = Encryptor.Decrypt(encryptedCard.Note, key)
                };
            }
            return null;
        }
    }
}
