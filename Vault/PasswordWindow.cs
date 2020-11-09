using System.Windows;
using System.Windows.Input;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle password memorizzate.
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private readonly IDialogListener listener = null;
        private readonly Password password = null;

        public const string DONE = "PasswordWindow.DONE";


        public PasswordWindow(IDialogListener listener, Password password)
        {
            InitializeComponent();
            this.listener = listener;
            this.password = password != null ? Core.Password.Decrypt(password, Global.Instance.Key) : null;
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            listener?.OnDialogAction(DialogAction.CANCEL);
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
            else Delete.Visibility = Visibility.Hidden;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (password == null) AddElement();
            else EditElement();
            listener?.OnDialogAction(DialogAction.ACTION, DONE);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            listener?.OnDialogAction(DialogAction.CANCEL);
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
            VaultDB.Instance.Passwords.AddRecord(Core.Password.Encrypt(password, Global.Instance.Key));
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
            VaultDB.Instance.Passwords.UpdateRecord(Core.Password.Encrypt(password, Global.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Passwords.RemoveRecord(password.ID);
            listener?.OnDialogAction(DialogAction.ACTION, DONE);
            Close();
        }
    }
}
