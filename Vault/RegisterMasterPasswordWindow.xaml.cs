using FullControls.SystemComponents;
using System;
using System.Windows;
using System.Windows.Input;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra di registrazione.
    /// </summary>
    public partial class RegisterMasterPasswordWindow : FlexWindow
    {
        public RegisterMasterPasswordWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (TrayIcon.Instance.WindowToShow == null && Application.Current.Windows.Count == 0) Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ConfirmCommand();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmCommand();
        }

        private void ConfirmCommand()
        {
            if (Password.PasswordLength > 0 && ConfirmPassword.PasswordLength > 0)
            {
                if (Utility.ComparePasswords(Password.SecurePassword, ConfirmPassword.SecurePassword))
                {
                    VaultDB.Context = new VaultDBContext(Settings.Instance.DBPath, Password.Password);
                    _ = VaultDB.Initialize();

                    if (Remember.IsChecked == true)
                    {
                        Settings.Instance.DBPassword = Password.Password;
                    }

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
