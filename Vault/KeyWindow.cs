using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra di conferma password.
    /// </summary>
    public partial class KeyWindow : Window
    {
        private int attempt = 0;

        public string Result { get; set; } = "";

        public const string NONE = "KeyWindow.NONE";
        public const string CONFIRMED = "KeyWindow.CONFIRMED";


        public KeyWindow()
        {
            InitializeComponent();
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (Password.GetSecurePassword().Length > 0)
            {
                CheckPasswordAndConfirm();
            }
            else
            {
                _ = new MessageWindow("Immettere la password!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
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

            User user = VaultDB.Instance.Users.GetRecord(Global.Instance.UserID);
            byte[] salt = Encryptor.ConvertToBytes(user.Password).Take(32).ToArray();
            byte[] pkey = Encryptor.GenerateKey(Password.GetSecurePassword(), salt);
            byte[] hashPkey = salt.Concat(Encryptor.GenerateKey(pkey, salt)).ToArray();
            if (user.Password.Equals(Encryptor.ConvertToString(hashPkey)))
            {
                Result = CONFIRMED;
                Close();
            }
            else
            {
                _ = new MessageWindow("Password errata!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
            }
        }
    }
}
