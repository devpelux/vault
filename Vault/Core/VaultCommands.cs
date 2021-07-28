using System.Windows.Input;

namespace Vault.Core
{
    public static class VaultCommands
    {
        private static readonly RoutedCommand _copyValue = new(nameof(CopyValue), typeof(VaultCommands));
        private static readonly RoutedCommand _replaceValue = new(nameof(ReplaceValue), typeof(VaultCommands));

        public static RoutedCommand CopyValue => _copyValue;
        public static RoutedCommand ReplaceValue => _replaceValue;
    }
}
