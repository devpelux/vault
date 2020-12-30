namespace Vault.Core
{
    public record Note(int ID, int User, int Category, bool RequestKey, string Title, string Subtitle, string Text)
        : ICategorizable, IDecryptable<Note>
    {
        public Note Encrypt(byte[] key)
            => this with
            {
                Title = Encryptor.Encrypt(Title, key),
                Subtitle = Encryptor.Encrypt(Subtitle, key),
                Text = Encryptor.Encrypt(Text, key)
            };

        public Note Decrypt(byte[] key)
            => this with
            {
                Title = Encryptor.Decrypt(Title, key),
                Subtitle = Encryptor.Decrypt(Subtitle, key),
                Text = Encryptor.Decrypt(Text, key)
            };

        public Note PreDecrypt(byte[] key)
            => this with
            {
                Title = Encryptor.Decrypt(Title, key),
                Subtitle = Encryptor.Decrypt(Subtitle, key)
            };
    }
}
