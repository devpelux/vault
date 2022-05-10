using FullControls.Controls;
using FullControls.SystemComponents;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for display and edit a password.
    /// </summary>
    public partial class PasswordWindow : AvalonWindow, IDialog
    {
        private readonly Password? password;
        private readonly List<Category> categories;

        private object? Result = null;

        /// <summary>
        /// Initializes a new <see cref="PasswordWindow"/> with the specified password.
        /// If the password is null, the window will create a new password, otherwise will display and edit the specified password.
        /// </summary>
        public PasswordWindow(Password? password)
        {
            InitializeComponent();
            this.password = password;
            categories = DB.Instance.Categories.GetAll();
        }

        /// <inheritdoc/>
        public object? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the password details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utility.LoadCategoryItems(PasswordCategory, (Style)FindResource("DarkComboBoxItemPlus"), categories);

            if (password != null)
            {
                PasswordRequestKey.IsChecked = password.IsLocked;
                PasswordLabel.Text = password.Account;
                PasswordUsername.Text = password.Username;
                PasswordKey.Password = password.Value;
                PasswordNote.Text = password.Notes;
                PasswordCategory.SelectedIndex = categories.FindIndex(category => category.Name == password.Category);
                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordCategory.SelectedIndex = 0;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        #region Commands

        private void CopyValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is TextBoxPlus textBox)
            {
                e.CanExecute = textBox.TextLength > 0;
            }
            else if (sender is PasswordBoxPlus passwordBox)
            {
                e.CanExecute = passwordBox.PasswordLength > 0;
            }
        }

        private void ReplaceValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText();
        }

        private void CopyValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is TextBoxPlus textBox) textBox.CopyAll();
            else if (sender is PasswordBoxPlus passwordBox) passwordBox.CopyAll();
        }

        private void ReplaceValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is TextBoxPlus textBox)
            {
                textBox.Clear();
                textBox.Paste();
            }
            else if (sender is PasswordBoxPlus passwordBox)
            {
                passwordBox.Clear();
                passwordBox.Paste();
            }
        }

        #endregion

        /// <summary>
        /// Executed when the cancel button is clicked.
        /// Closes the window.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// Executed when the ok button is clicked.
        /// Edits the password if is not null, otherwise creates a new password.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (password == null) AddPassword();
            else EditPassword();

            Result = "edit";
            Close();
        }

        /// <summary>
        /// Adds a new password.
        /// </summary>
        private void AddPassword()
        {
            string category = categories[PasswordCategory.SelectedIndex].Name;
            string account = PasswordLabel.Text;
            long timestamp = -1;
            string? username = PasswordUsername.Text;
            string value = PasswordKey.Password;
            string? notes = PasswordNote.Text;
            bool isViolated = false;
            bool isLocked = PasswordRequestKey.IsChecked ?? false;

            Password newPassword = new(category, account, timestamp, username, value, notes, isViolated, isLocked);

            DB.Instance.Passwords.Add(newPassword);
        }

        /// <summary>
        /// Edit the password.
        /// </summary>
        private void EditPassword()
        {
            if (password == null) return;

            int id = password.Id;
            string category = categories[PasswordCategory.SelectedIndex].Name;
            string account = PasswordLabel.Text;
            long timestamp = -1;
            string? username = PasswordUsername.Text;
            string value = PasswordKey.Password;
            string? notes = PasswordNote.Text;
            bool isViolated = false;
            bool isLocked = PasswordRequestKey.IsChecked ?? false;

            Password newPassword = new(id, category, account, timestamp, username, value, notes, isViolated, isLocked);

            DB.Instance.Passwords.Update(newPassword);
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the password if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (password == null) return;

            DB.Instance.Passwords.Remove(password.Id);

            Result = "edit";
            Close();
        }
    }
}