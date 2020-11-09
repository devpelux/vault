using System.Security;

namespace Vault.Core
{
    public static class Extensions
    {
        public static void AppendString(this SecureString ss, string s)
        {
            foreach (char c in s) ss.AppendChar(c);
        }
    }
}
