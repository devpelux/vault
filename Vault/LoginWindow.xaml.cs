using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.CustomControls;
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Finestra di login.
    /// </summary>
    public partial class LoginWindow : Window
    {
        private int attempt = 0;


        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.User != "") Remember.IsChecked = true;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text.Length > 0 && Password.GetSecurePassword().Length > 0)
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
                byte[] pkey = Encryptor.GenerateKey(Password.GetSecurePassword(), salt);
                byte[] hashPkey = salt.Concat(Encryptor.GenerateKey(pkey, salt)).ToArray();
                if (user.Password.Equals(Encryptor.ConvertToString(hashPkey)))
                {
                    Global.Instance.UserID = user.ID;
                    Global.Instance.Username = user.Username;
                    Global.Instance.Key = Encryptor.ConvertToBytes(Encryptor.Decrypt(user.Key, pkey));

                    if (Remember.IsChecked ?? false) Settings.Default.User = Global.Instance.Username;
                    else Settings.Default.User = "";

                    new Home().Show();
                    Close();
                    return;
                }
            }

            _ = new DialogWindow(new MessageWindow("Username o password errati!", "Errore", MessageBoxImage.Exclamation)).Show();
        }
    }
}
