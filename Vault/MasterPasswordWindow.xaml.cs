using FullControls;
using System.Threading;
using System.Windows;
using Vault.Core;
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Finestra di conferma password.
    /// </summary>
    public partial class MasterPasswordWindow : EWindow
    {
        private int attempt = 0;


        public MasterPasswordWindow()
        {
            InitializeComponent();
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

            VaultDB.Context = new VaultDBContext(Settings.Default.DBPath, Password.Password);
            if (VaultDB.Initialize())
            {
                if (Remember.IsChecked == true) Settings.Default.DBSavedPassword = Password.Password;
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
