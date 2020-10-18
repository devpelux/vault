namespace Vault
{
    public interface IMessageReceiver
    {
        void ReceiveMessage(string message, object obj);
    }
}
