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
                                        "UserID INTEGER NOT NULL, " +
                                        "Title TEXT NOT NULL, " +
                                        "Category TEXT NOT NULL, " +
                                        "Details TEXT NOT NULL, " +
                                        "RequestKey INTEGER NOT NULL, " +
                                        "Text TEXT NOT NULL, " +
                                        "PRIMARY KEY(ID AUTOINCREMENT), " +
                                        "FOREIGN KEY(UserID) REFERENCES Users(ID)" +
                                        ");";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void DeleteTable()
        {
            string command = "DROP TABLE IF EXISTS Notes;";
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

        public void AddRecord(Note record)
        {
            string command = "INSERT INTO Notes (UserID, Title, Category, Details, RequestKey, Text) " +
                                    "VALUES (@UserID, @Title, @Category, @Details, @RequestKey, @Text);";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Text", record.Text);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public void RemoveRecord(int id)
        {
            string command = "DELETE FROM Notes WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public List<Note> GetAllRecords()
        {
            List<Note> records = new List<Note>();
            string command = "SELECT * FROM Notes";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public Note GetRecord(int id)
        {
            string command = "SELECT * FROM Notes WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            if (reader.Read()) return ReadElementFromReader(reader);
            return null;
        }

        public List<Note> GetRecords(int userId)
        {
            List<Note> records = new List<Note>();
            string command = "SELECT Notes.ID, " +
                                    "Notes.UserID, " +
                                    "Notes.Title, " +
                                    "Notes.Category, " +
                                    "Notes.Details, " +
                                    "Notes.RequestKey, " +
                                    "Notes.Text " +
                                    "FROM Notes " +
                                    "JOIN Users ON Notes.UserID = Users.ID " +
                                    "WHERE Users.ID = @UserID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", userId);
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public List<Note> GetRecords(string title)
        {
            List<Note> records = new List<Note>();
            string command = "SELECT * FROM Notes WHERE Title LIKE @Title;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@Title", $"%{title}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public List<Note> GetRecords(string title, int userId)
        {
            List<Note> records = new List<Note>();
            string command = "SELECT Notes.ID, " +
                                    "Notes.UserID, " +
                                    "Notes.Title, " +
                                    "Notes.Category, " +
                                    "Notes.Details, " +
                                    "Notes.RequestKey, " +
                                    "Notes.Text " +
                                    "FROM Notes " +
                                    "JOIN Users ON Notes.UserID = Users.ID " +
                                    "WHERE Users.ID = @UserID AND Notes.Title LIKE @Title;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@UserID", userId);
            query.Parameters.AddWithValue("@Title", $"%{title}%");
            query.Prepare();
            SqliteDataReader reader = query.ExecuteReader();
            while (reader.Read()) records.Add(ReadElementFromReader(reader));
            return records;
        }

        public void UpdateRecord(Note record)
        {
            string command = "UPDATE Notes " +
                                    "SET UserID = @UserID, " +
                                        "Title = @Title, " +
                                        "Category = @Category, " +
                                        "Details = @Details, " +
                                        "RequestKey = @RequestKey, " +
                                        "Text = @Text " +
                                    "WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", record.ID);
            query.Parameters.AddWithValue("@UserID", record.UserID);
            query.Parameters.AddWithValue("@Title", record.Title);
            query.Parameters.AddWithValue("@Category", record.Category);
            query.Parameters.AddWithValue("@Details", record.Details);
            query.Parameters.AddWithValue("@RequestKey", record.RequestKey ? 1 : 0);
            query.Parameters.AddWithValue("@Text", record.Text);
            query.Prepare();
            query.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            string command = "SELECT COUNT(*) FROM Notes WHERE ID = @ID;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Parameters.AddWithValue("@ID", id);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar()) > 0;
        }

        public int Count()
        {
            string command = "SELECT COUNT(*) FROM Notes;";
            SqliteCommand query = new SqliteCommand(command, VaultDB.Connection);
            query.Prepare();
            return Convert.ToInt32(query.ExecuteScalar());
        }

        private Note ReadElementFromReader(SqliteDataReader reader)
        {
            return new Note
            {
                ID = reader.GetInt32(0),
                UserID = reader.GetInt32(1),
                Title = reader.GetString(2),
                Category = reader.GetString(3),
                Details = reader.GetString(4),
                RequestKey = reader.GetInt32(5) == 1,
                Text = reader.GetString(6)
            };
        }

        public static Note Encrypt(Note note, byte[] key)
        {
            if (note != null)
            {
                return new Note
                {
                    ID = note.ID,
                    UserID = note.UserID,
                    Title = note.Title,
                    Category = note.Category,
                    Details = note.Details,
                    RequestKey = note.RequestKey,
                    Text = Encryptor.Encrypt(note.Text, key)
                };
            }
            return null;
        }

        public static Note Decrypt(Note encryptedNote, byte[] key)
        {
            if (encryptedNote != null)
            {
                return new Note
                {
                    ID = encryptedNote.ID,
                    UserID = encryptedNote.UserID,
                    Title = encryptedNote.Title,
                    Category = encryptedNote.Category,
                    Details = encryptedNote.Details,
                    RequestKey = encryptedNote.RequestKey,
                    Text = Encryptor.Decrypt(encryptedNote.Text, key)
                };
            }
            return null;
        }
    }
}
