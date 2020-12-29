using FullControls;
using System;
using System.Threading;
using System.Windows;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra di conferma password.
    /// </summary>
    public partial class MasterPasswordWindow : EWindow
    {
        private int attempt = 0;
        private readonly bool DisplayError;


        public MasterPasswordWindow(bool displayError = false)
        {
            InitializeComponent();
            DisplayError = displayError;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (DisplayError)
            {
                _ = new DialogWindow(new MessageWindow("La password memorizzata è errata o il file di dati è corrotto!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (Password.PasswordLength > 0)
            {
                CheckPasswordAndOpenDatabase();
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("Immettere una password!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }

        private void CheckPasswordAndOpenDatabase()
        {
            attempt += 1;
            if (attempt > 4) Thread.Sleep(2000);

            VaultDB.Context = new VaultDBContext(SettingsWrapper.DBPath, Password.Password);
            if (VaultDB.Initialize())
            {
                if (Remember.IsChecked == true)
                {
                    SettingsWrapper.DBSavedPassword = Password.Password;
                }

                if (VaultDB.Instance.Users.Count() > 0)
                {
                    new LoginWindow().Show();
                }
                else
                {
                    new RegisterWindow().Show();
                }
                Close();
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("La password è errata o il file di dati è corrotto!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
