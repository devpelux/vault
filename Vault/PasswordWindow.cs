using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.CustomControls;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle password memorizzate.
    /// </summary>
    public partial class PasswordWindow : Window, IDialogWindow
    {
        private readonly Password password = null;

        private string Result = "";

        public const string NONE = "PasswordWindow.NONE";
        public const string EDIT = "PasswordWindow.EDIT";


        public PasswordWindow(Password password)
        {
            InitializeComponent();
            this.password = password != null ? Passwords.Decrypt(password, Global.Instance.Key) : null;
        }

        public string GetResult() => Result;

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (password != null)
            {
                Label.Text = password.Title;
                Website.Text = password.Website;
                Password.SetPassword(password.Key);
                Username.Text = password.Username;
                Details.Text = password.Details;
                Note.Text = password.Note;
                RequestKey.IsChecked = password.RequestKey;
                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                Delete.Visibility = Visibility.Hidden;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (password == null) AddElement();
            else EditElement();
            Result = EDIT;
            Close();
        }

        private void AddElement()
        {
            Password password = new Password();
            password.UserID = Global.Instance.UserID;
            password.Title = Label.Text;
            password.Category = "Generic";
            password.Website = Website.Text;
            password.Username = Username.Text;
            password.Key = Password.GetPassword();
            password.Details = Details.Text;
            password.Note = Note.Text;
            password.RequestKey = RequestKey.IsChecked ?? false;
            VaultDB.Instance.Passwords.AddRecord(Passwords.Encrypt(password, Global.Instance.Key));
        }

        private void EditElement()
        {
            password.UserID = Global.Instance.UserID;
            password.Title = Label.Text;
            password.Category = "Generic";
            password.Website = Website.Text;
            password.Username = Username.Text;
            password.Key = Password.GetPassword();
            password.Details = Details.Text;
            password.Note = Note.Text;
            password.RequestKey = RequestKey.IsChecked ?? false;
            VaultDB.Instance.Passwords.UpdateRecord(Passwords.Encrypt(password, Global.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Passwords.RemoveRecord(password.ID);
            Result = EDIT;
            Close();
        }
    }
}
