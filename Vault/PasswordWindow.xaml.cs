using FullControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle password memorizzate.
    /// </summary>
    public partial class PasswordWindow : EWindow, IDialog
    {
        private readonly Password password;
        private readonly List<Category> categories;

        private object Result = null;

        public const string NONE = "PasswordWindow.NONE";
        public const string EDIT = "PasswordWindow.EDIT";


        public PasswordWindow(Password password, List<Category> categories)
        {
            InitializeComponent();
            this.password = password?.Decrypt(Session.Instance.Key);
            this.categories = categories;
        }

        public object GetResult() => Result;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Result == null) Result = NONE;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utility.LoadCategoryItems(PasswordCategory, (Style)FindResource("EComboBoxItemDark"), categories);
            if (password != null)
            {
                PasswordRequestKey.IsChecked = password.RequestKey;
                PasswordLabel.Text = password.Label;
                PasswordDescription.Text = password.Description;
                PasswordUrl.Text = password.Url;
                PasswordUsername.Text = password.Username;
                PasswordKey.Password = password.Key;
                PasswordNote.Text = password.Note;
                PasswordCategory.SelectedIndex = categories.FindIndex(c => c.ID == password.Category);
                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordCategory.SelectedIndex = 0;
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
            Password password = new(
                    Passwords.NewID,
                    Session.Instance.UserID,
                    categories[PasswordCategory.SelectedIndex].ID,
                    PasswordRequestKey.IsChecked ?? false,
                    PasswordLabel.Text,
                    PasswordDescription.Text,
                    PasswordUrl.Text,
                    PasswordUsername.Text,
                    PasswordKey.Password,
                    PasswordNote.Text
                );
            VaultDB.Instance.Passwords.AddRecord(password.Encrypt(Session.Instance.Key));
        }

        private void EditElement()
        {
            Password editedPassword = password with
            {
                Category = categories[PasswordCategory.SelectedIndex].ID,
                RequestKey = PasswordRequestKey.IsChecked ?? false,
                Label = PasswordLabel.Text,
                Description = PasswordDescription.Text,
                Url = PasswordUrl.Text,
                Username = PasswordUsername.Text,
                Key = PasswordKey.Password,
                Note = PasswordNote.Text
            };
            VaultDB.Instance.Passwords.UpdateRecord(editedPassword.Encrypt(Session.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Passwords.RemoveRecord(password.ID);
            Result = EDIT;
            Close();
        }
    }
}
