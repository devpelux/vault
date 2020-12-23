using FullControls;
using System.Windows;
using Vault.Core;
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Finestra di registrazione.
    /// </summary>
    public partial class RegisterMasterPasswordWindow : EWindow
    {
        public RegisterMasterPasswordWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (Password.PasswordLength > 0 && ConfirmPassword.PasswordLength > 0)
            {
                if (Utility.ComparePasswords(Password.SecurePassword, ConfirmPassword.SecurePassword))
                {
                    VaultDB.Context = new VaultDBContext(Settings.Default.DBPath, Password.Password);
                    if (Remember.IsChecked == true) Settings.Default.DBSavedPassword = Password.Password;
                    new RegisterWindow().Show();
                    Close();
                }
                else
                {
                    _ = new DialogWindow(new MessageWindow("Le password non corrispondono!", "Errore", MessageBoxImage.Exclamation)).Show();
                }
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("Immettere una password!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
