namespace Vault.Core
{
    public record Password(int ID, int User, int Category, bool RequestKey, string Label, string Description, string Url, string Username, string Key, string Note)
        : ICategorizable, IDecryptable<Password>
    {
        public Password Encrypt(byte[] key)
            => this with
            {
                Label = Encryptor.Encrypt(Label, key),
                Description = Encryptor.Encrypt(Description, key),
                Url = Encryptor.Encrypt(Url, key),
                Username = Encryptor.Encrypt(Username, key),
                Key = Encryptor.Encrypt(Key, key),
                Note = Encryptor.Encrypt(Note, key)
            };

        public Password Decrypt(byte[] key)
            => this with
            {
                Label = Encryptor.Decrypt(Label, key),
                Description = Encryptor.Decrypt(Description, key),
                Url = Encryptor.Decrypt(Url, key),
                Username = Encryptor.Decrypt(Username, key),
                Key = Encryptor.Decrypt(Key, key),
                Note = Encryptor.Decrypt(Note, key)
            };

        public Password PreDecrypt(byte[] key)
            => this with
            {
                Label = Encryptor.Decrypt(Label, key),
                Url = Encryptor.Decrypt(Url, key)
            };
    }
}
