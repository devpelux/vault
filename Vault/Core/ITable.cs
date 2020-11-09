namespace Vault.Core
{
    public interface ITable
    {
        void CreateTable();
        void DeleteTable();
        void UpdateTable(int newVersion, int oldVersion);
    }
}
