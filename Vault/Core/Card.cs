namespace Vault.Core
{
    public record Card(int ID, int User, int Category, bool RequestKey, string Label, string Description,
        string Owner, string Type, string Number, string SecureCode, string Expiration, string Note)
        : ICategorizable, IDecryptable<Card>
    {
        public Card Encrypt(byte[] key)
            => this with
            {
                Label = Encryptor.Encrypt(Label, key),
                Description = Encryptor.Encrypt(Description, key),
                Owner = Encryptor.Encrypt(Owner, key),
                Type = Encryptor.Encrypt(Type, key),
                Number = Encryptor.Encrypt(Number, key),
                SecureCode = Encryptor.Encrypt(SecureCode, key),
                Expiration = Encryptor.Encrypt(Expiration, key),
                Note = Encryptor.Encrypt(Note, key)
            };

        public Card Decrypt(byte[] key)
            => this with
            {
                Label = Encryptor.Decrypt(Label, key),
                Description = Encryptor.Decrypt(Description, key),
                Owner = Encryptor.Decrypt(Owner, key),
                Type = Encryptor.Decrypt(Type, key),
                Number = Encryptor.Decrypt(Number, key),
                SecureCode = Encryptor.Decrypt(SecureCode, key),
                Expiration = Encryptor.Decrypt(Expiration, key),
                Note = Encryptor.Decrypt(Note, key)
            };

        public Card PreDecrypt(byte[] key)
            => this with
            {
                Label = Encryptor.Decrypt(Label, key),
                Type = Encryptor.Decrypt(Type, key)
            };
    }
}
