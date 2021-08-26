using System.Windows.Input;

namespace Vault.Core
{
    public static class VaultCommands
    {
        public static RoutedCommand CopyValue { get; } = new(nameof(CopyValue), typeof(VaultCommands));
        public static RoutedCommand ReplaceValue { get; } = new(nameof(ReplaceValue), typeof(VaultCommands));
    }
}
