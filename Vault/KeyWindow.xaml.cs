using FullControls.SystemComponents;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Finestra di conferma password.
    /// </summary>
    public partial class KeyWindow : FlexWindow, IDialog
    {
        private int attempt = 0;

        private object Result = null;

        public const string NONE = "KeyWindow.NONE";
        public const string CONFIRMED = "KeyWindow.CONFIRMED";


        public KeyWindow()
        {
            InitializeComponent();
        }

        public object GetResult() => Result;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Result == null) Result = NONE;
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
            if (Password.SecurePassword.Length > 0)
            {
                CheckPasswordAndConfirm();
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("Immettere la password!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void CheckPasswordAndConfirm()
        {
            attempt += 1;
            if (attempt > 4) Thread.Sleep(2000);

            User user = VaultDB.Instance.Users.GetRecord(Session.Instance.UserID);
            byte[] salt = Encryptor.ConvertToBytes(user.Password).Take(32).ToArray();
            byte[] pkey = Encryptor.GenerateKey(Password.SecurePassword, salt);
            byte[] hashPkey = salt.Concat(Encryptor.GenerateKey(pkey, salt)).ToArray();
            if (user.Password.Equals(Encryptor.ConvertToString(hashPkey)))
            {
                Result = CONFIRMED;
                Close();
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("Password errata!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }
    }
}
