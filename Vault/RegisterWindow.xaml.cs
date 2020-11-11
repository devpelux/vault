using System.Linq;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.CustomControls;
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Finestra di registrazione.
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text.Length > 0 && Password.GetSecurePassword().Length > 0 && ConfirmPassword.GetSecurePassword().Length > 0)
            {
                if (!Username.Text.Contains(" "))
                {
                    if (!VaultDB.Instance.Users.Exists(Username.Text))
                    {
                        byte[] salt = Encryptor.GenerateSalt();
                        string hashPassword = Encryptor.ConvertToString(Encryptor.GenerateKey(Password.GetSecurePassword(), salt, 1000));
                        string hashConfirmPassword = Encryptor.ConvertToString(Encryptor.GenerateKey(ConfirmPassword.GetSecurePassword(), salt, 1000));

                        if (hashPassword.Equals(hashConfirmPassword))
                        {
                            RegisterUserAndLogin();
                        }
                        else
                        {
                            _ = new DialogWindow(new MessageWindow("Le password non corrispondono!", "Errore", MessageBoxImage.Exclamation)).Show();
                        }
                    }
                    else
                    {
                        _ = new DialogWindow(new MessageWindow("Username già esistente!", "Errore", MessageBoxImage.Exclamation)).Show();
                    }
                }
                else
                {
                    _ = new DialogWindow(new MessageWindow("Username non valido!", "Errore", MessageBoxImage.Exclamation)).Show();
                }
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("Immettere username e password!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }

        private void RegisterUserAndLogin()
        {
            byte[] key = Encryptor.GenerateKey();
            byte[] salt = Encryptor.GenerateSalt();
            byte[] pkey = Encryptor.GenerateKey(Password.GetSecurePassword(), salt);
            byte[] hashPkey = salt.Concat(Encryptor.GenerateKey(pkey, salt)).ToArray();
            string cipherKey = Encryptor.Encrypt(key, pkey);

            VaultDB.Instance.Users.AddRecord(new User(Users.NewID, Username.Text, Encryptor.ConvertToString(hashPkey), cipherKey));

            Global.Instance.UserID = VaultDB.Instance.Users.GetRecord(Username.Text).ID;
            Global.Instance.Username = Username.Text;
            Global.Instance.Key = key;

            if (Remember.IsChecked ?? false) Settings.Default.User = Global.Instance.Username;

            new Home().Show();
            Close();
        }
    }
}
