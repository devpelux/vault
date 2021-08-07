using FullControls.SystemComponents;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra di registrazione.
    /// </summary>
    public partial class RegisterWindow : FlexWindow
    {
        private const string DEFAULT_CATEGORY = "Default";


        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) RegisterCommand();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterCommand();
        }

        private void RegisterCommand()
        {
            if (Username.TextLength > 0 && Password.PasswordLength > 0 && ConfirmPassword.PasswordLength > 0)
            {
                if (!Username.Text.Contains(" "))
                {
                    if (!VaultDB.Instance.Users.Exists(Username.Text))
                    {
                        if (Utility.ComparePasswords(Password.SecurePassword, ConfirmPassword.SecurePassword))
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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void RegisterUserAndLogin()
        {
            byte[] key = Encryptor.GenerateKey();
            byte[] salt = Encryptor.GenerateSalt();
            byte[] pkey = Encryptor.GenerateKey(Password.SecurePassword, salt);
            byte[] hashPkey = salt.Concat(Encryptor.GenerateKey(pkey, salt)).ToArray();
            string cipherKey = Encryptor.Encrypt(key, pkey);

            VaultDB.Instance.Users.AddRecord(new User(Users.NewID, Username.Text, Encryptor.ConvertToString(hashPkey), cipherKey));

            Session.Instance.UserID = VaultDB.Instance.Users.GetRecord(Username.Text).ID;
            Session.Instance.Username = Username.Text;
            Session.Instance.Key = key;

            AddDefaultCategory();

            if (Remember.IsChecked == true)
            {
                Settings.Instance.User = Session.Instance.Username;
            }

            new Home().Show();
            Close();
        }

        private static void AddDefaultCategory()
            => VaultDB.Instance.Categories.AddRecord(new Category(Categories.NewID, Session.Instance.UserID, DEFAULT_CATEGORY, true));
    }
}
