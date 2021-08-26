using FullControls.SystemComponents;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra di login.
    /// </summary>
    public partial class LoginWindow : FlexWindow
    {
        private int attempt = 0;


        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Instance.User != null)
            {
                Username.Text = Settings.Instance.User;
                Remember.IsChecked = true;
            }
            TrayIcon.Instance.WindowToShow = null;
        }

        private void Window_CloseCommandExecuting(object sender, EventArgs e)
        {
            if (Settings.Instance.HideOnClose == true) TrayIcon.Instance.WindowToShow = nameof(LoginWindow);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (TrayIcon.Instance.WindowToShow == null && Application.Current.Windows.Count == 0) Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LoginCommand();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginCommand();
        }

        private void LoginCommand()
        {
            if (Username.TextLength > 0 && Password.PasswordLength > 0)
            {
                CheckPasswordAndLogin();
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("Immettere username e password!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            Close();
        }

        private void CheckPasswordAndLogin()
        {
            attempt += 1;
            if (attempt > 4) Thread.Sleep(2000);

            User user = VaultDB.Instance.Users.GetRecord(Username.Text);
            if (user != null)
            {
                byte[] salt = Encryptor.ConvertToBytes(user.Password).Take(32).ToArray();
                byte[] pkey = Encryptor.GenerateKey(Password.SecurePassword, salt);
                byte[] hashPkey = salt.Concat(Encryptor.GenerateKey(pkey, salt)).ToArray();
                if (user.Password.Equals(Encryptor.ConvertToString(hashPkey)))
                {
                    Session.ClearInstance();
                    Session.Instance.UserID = user.ID;
                    Session.Instance.Username = user.Username;
                    Session.Instance.Key = Encryptor.ConvertToBytes(Encryptor.Decrypt(user.Key, pkey));

                    if (Remember.IsChecked == true) Settings.Instance.User = Session.Instance.Username;
                    else Settings.Instance.User = null;

                    new Home().Show();
                    TrayIcon.Instance.VaultStatus = VaultStatus.Unlocked;
                    Close();
                    return;
                }
            }

            _ = new DialogWindow(new MessageWindow("Username o password errati!", "Errore", MessageBoxImage.Exclamation)).Show();
        }
    }
}
