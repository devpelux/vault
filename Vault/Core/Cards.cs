using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vault.Core
{
    public record Card(int ID, int User, int Category, bool RequestKey, string Label, string Description,
        string Owner, string Type, string Number, string SecureCode, string Expiration, string Note);

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

        public List<Card> GetRecords(string label)
        {
            List<Card> records = new();
            string command = "SELECT * FROM Cards WHERE Label LIKE @Label;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Title", $"%{label}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public List<Card> GetRecords(string label, int user)
        {
            List<Card> records = new();
            string command = "SELECT * FROM Cards WHERE User = @User AND Label LIKE @Label;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Parameters.AddWithValue("@Label", $"%{label}%");
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

        public static Card Encrypt(Card card, byte[] key)
            => card with
            {
                Label = Encryptor.Encrypt(card.Label, key),
                Description = Encryptor.Encrypt(card.Description, key),
                Owner = Encryptor.Encrypt(card.Owner, key),
                Type = Encryptor.Encrypt(card.Type, key),
                Number = Encryptor.Encrypt(card.Number, key),
                SecureCode = Encryptor.Encrypt(card.SecureCode, key),
                Expiration = Encryptor.Encrypt(card.Expiration, key),
                Note = Encryptor.Encrypt(card.Note, key)
            };

        public static Card Decrypt(Card encryptedCard, byte[] key)
            => encryptedCard with
            {
                Label = Encryptor.Decrypt(encryptedCard.Label, key),
                Description = Encryptor.Decrypt(encryptedCard.Description, key),
                Owner = Encryptor.Decrypt(encryptedCard.Owner, key),
                Type = Encryptor.Decrypt(encryptedCard.Type, key),
                Number = Encryptor.Decrypt(encryptedCard.Number, key),
                SecureCode = Encryptor.Decrypt(encryptedCard.SecureCode, key),
                Expiration = Encryptor.Decrypt(encryptedCard.Expiration, key),
                Note = Encryptor.Decrypt(encryptedCard.Note, key)
            };

        public static List<Card> DecryptForPreview(List<Card> encryptedCards, byte[] key)
            => encryptedCards.Select(card => DecryptForPreview(card, key)).ToList();

        private static Card DecryptForPreview(Card encryptedCard, byte[] key)
            => encryptedCard with
            {
                Label = Encryptor.Decrypt(encryptedCard.Label, key),
                Type = Encryptor.Decrypt(encryptedCard.Type, key)
            };

        public static List<CategoryValues> GroupByCategories(List<Card> elements, List<Category> categories)
            => categories.Select(category =>
            {
                List<Card> filteredElements = elements.FindAll(e => e.Category == category.ID);
                return new CategoryValues(category, filteredElements, filteredElements.Count);
            }).ToList();
    }
}
