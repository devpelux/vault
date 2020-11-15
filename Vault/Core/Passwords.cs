using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vault.Core
{
    public record Password(int ID, int User, int Category, bool RequestKey, string Label, string Description, string Url, string Username, string Key, string Note);

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
                                        "User INTEGER NOT NULL, " +
                                        "Category INTEGER NOT NULL, " +
                                        "RequestKey INTEGER NOT NULL, " +
                                        "Label TEXT NOT NULL, " +
                                        "Description TEXT NOT NULL, " +
                                        "Url TEXT NOT NULL, " +
                                        "Username TEXT NOT NULL, " +
                                        "Key TEXT NOT NULL, " +
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
            string command = "INSERT INTO Passwords (User, Category, RequestKey, Label, Description, Url, Username, Key, Note) " +
                                    "VALUES (@User, @Category, @RequestKey, @Label, @Description, @Url, @Username, @Key, @Note);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Label", record.Label);
            query.Parameters.AddWithValue("@Description", record.Description);
            query.Parameters.AddWithValue("@Url", record.Url);
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
            List<Password> records = new();
            string command = "SELECT * FROM Passwords";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public Password GetRecord(int id)
        {
            string command = "SELECT * FROM Passwords WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadRecord(reader);
            return null;
        }

        public List<Password> GetRecords(int user)
        {
            List<Password> records = new();
            string command = "SELECT * FROM Passwords WHERE User = @User;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public List<Password> GetRecords(string label)
        {
            List<Password> records = new();
            string command = "SELECT * FROM Passwords WHERE Label LIKE @Label;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Label", $"%{label}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public List<Password> GetRecords(string label, int user)
        {
            List<Password> records = new();
            string command = "SELECT * FROM Passwords WHERE User = @User AND Label LIKE @Label;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@User", user);
            query.Parameters.AddWithValue("@Label", $"%{label}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadRecord(reader));
            return records;
        }

        public void UpdateRecord(Password record)
        {
            string command = "UPDATE Passwords " +
                                    "SET User = @User, " +
                                        "Category = @Category, " +
                                        "RequestKey = @RequestKey, " +
                                        "Label = @Label, " +
                                        "Description = @Description, " +
                                        "Url = @Url, " +
                                        "Username = @Username, " +
                                        "Key = @Key, " +
                                        "Note = @Note " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@User", record.User);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Label", record.Label);
            query.Parameters.AddWithValue("@Description", record.Description);
            query.Parameters.AddWithValue("@Url", record.Url);
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

        private static Password ReadRecord(SqliteDataReader reader)
            => new Password
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
                reader.GetString(9)
            );

        public static Password Encrypt(Password password, byte[] key)
            => password with
            {
                Label = Encryptor.Encrypt(password.Label, key),
                Description = Encryptor.Encrypt(password.Description, key),
                Url = Encryptor.Encrypt(password.Url, key),
                Username = Encryptor.Encrypt(password.Username, key),
                Key = Encryptor.Encrypt(password.Key, key),
                Note = Encryptor.Encrypt(password.Note, key)
            };

        public static Password Decrypt(Password encryptedPassword, byte[] key)
            => encryptedPassword with
            {
                Label = Encryptor.Decrypt(encryptedPassword.Label, key),
                Description = Encryptor.Decrypt(encryptedPassword.Description, key),
                Url = Encryptor.Decrypt(encryptedPassword.Url, key),
                Username = Encryptor.Decrypt(encryptedPassword.Username, key),
                Key = Encryptor.Decrypt(encryptedPassword.Key, key),
                Note = Encryptor.Decrypt(encryptedPassword.Note, key)
            };

        public static List<Password> DecryptForPreview(List<Password> encryptedPasswords, byte[] key)
            => encryptedPasswords.Select(password => DecryptForPreview(password, key)).ToList();

        private static Password DecryptForPreview(Password encryptedPassword, byte[] key)
            => encryptedPassword with
            {
                Label = Encryptor.Decrypt(encryptedPassword.Label, key),
                Url = Encryptor.Decrypt(encryptedPassword.Url, key)
            };

        public static List<CategoryValues> GroupByCategories(List<Password> elements, List<Category> categories)
            => categories.Select(category =>
            {
                List<Password> filteredElements = elements.FindAll(e => e.Category == category.ID);
                return new CategoryValues(category, filteredElements, filteredElements.Count);
            }).ToList();
    }
}
