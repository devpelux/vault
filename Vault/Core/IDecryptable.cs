namespace Vault.Core
{
    public interface IDecryptable<T>
    {
        T Encrypt(byte[] key);
        T Decrypt(byte[] key);
        T PreDecrypt(byte[] key);
    }
}
