using CoreTools.Extensions;
using FullControls.SystemComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using Vault.Core;
using Vault.Core.Controls;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using Vault.Properties;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for display and edit a password.
    /// </summary>
    public partial class PasswordWindow : AvalonWindow, IDialog<string>
    {
        private readonly Password? password;
        private readonly List<Category> categories;

        /// <summary>
        /// Result: "edited", "deleted", null = nothing. (default: null)
        /// </summary>
        private string? Result = null;

        /// <summary>
        /// Initializes a new <see cref="PasswordWindow"/> with the specified password.
        /// If the password is null, the window will create a new password, otherwise will display and edit the specified password.
        /// </summary>
        public PasswordWindow(Password? password)
        {
            InitializeComponent();

            this.password = password;
            categories = DB.Instance.Categories.GetAll();

            //Adds the field commands
            FieldCommands.AddFieldCommands(CommandBindings);
        }

        /// <inheritdoc/>
        public string? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the password details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Utility.LoadCategoryItems(PasswordCategory, (Style)FindResource("DarkComboBoxItemPlus"), categories);

            if (password != null)
            {
                PasswordCategory.SelectedIndex = categories.FindIndex(category => category.Name == password.Category);

                PasswordAccount.Text = password.Account;
                PasswordUsername.Text = password.Username;
                PasswordValue.Password = password.Value;
                PasswordNotes.Text = password.Notes;
                Violated.IsChecked = password.IsViolated;

                DateTime localTime = DateTimeOffset.FromUnixTimeSeconds(password.Timestamp).LocalDateTime;
                PasswordTimestampYear.Text = localTime.Year.ToString();
                PasswordTimestampMonth.Text = localTime.Month.ToString();
                PasswordTimestampDay.Text = localTime.Day.ToString();

                Reauthenticate.IsChecked = password.IsLocked;

                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordCategory.SelectedIndex = 0;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Executed when the save button is clicked.
        /// Edits the password if is not null, otherwise creates a new password.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (password == null) AddPassword();
                else EditPassword();

                Result = "edited";
                Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                new MessageWindow(Strings.InvalidDate, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
            }
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the password if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (password == null) return;

            if (new ConfirmWindow(Strings.ConfirmDelete, Strings.Warning, MessageBoxImage.Question).ShowDialogForResult<bool>())
            {
                DB.Instance.Passwords.Remove(password.Id);

                Result = "deleted";
                Close();
            }
        }

        /// <summary>
        /// Executed when the now button is clicked.
        /// Sets the date to now.
        /// </summary>
        private void PasswordTimestampNow_Click(object sender, RoutedEventArgs e)
        {
            DateTime localTime = DateTimeOffset.UtcNow.LocalDateTime;
            PasswordTimestampYear.Text = localTime.Year.ToString();
            PasswordTimestampMonth.Text = localTime.Month.ToString();
            PasswordTimestampDay.Text = localTime.Day.ToString();
        }

        /// <summary>
        /// Adds a new password.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private void AddPassword()
        {
            string category = categories[PasswordCategory.SelectedIndex].Name;
            string account = PasswordAccount.Text;
            string username = PasswordUsername.Text;
            string value = PasswordValue.Password;

            int year = PasswordTimestampYear.Text.ToInt(-1);
            int month = PasswordTimestampMonth.Text.ToInt(-1);
            int day = PasswordTimestampDay.Text.ToInt(-1);

            long timestamp;

            if (year != -1 || month != -1 || day != -1)
            {
                DateTimeOffset localTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), Math.Max(day, 1), 0, 0, 0, DateTimeKind.Local);
                timestamp = localTime.ToUnixTimeSeconds();
            }
            else
            {
                throw new ArgumentException("Invalid date.");
            }

            string? notes = PasswordNotes.Text;
            bool isViolated = Violated.IsChecked ?? false;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Password newPassword = new(category, account, timestamp, username, value, notes, isViolated, isLocked);

            DB.Instance.Passwords.Add(newPassword);
        }

        /// <summary>
        /// Edits the password.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private void EditPassword()
        {
            if (password == null) return;

            int id = password.Id;

            string category = categories[PasswordCategory.SelectedIndex].Name;
            string account = PasswordAccount.Text;
            string username = PasswordUsername.Text;
            string value = PasswordValue.Password;

            int year = PasswordTimestampYear.Text.ToInt(-1);
            int month = PasswordTimestampMonth.Text.ToInt(-1);
            int day = PasswordTimestampDay.Text.ToInt(-1);

            long timestamp;

            if (year != -1 || month != -1 || day != -1)
            {
                DateTimeOffset localTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), Math.Max(day, 1), 0, 0, 0, DateTimeKind.Local);
                timestamp = localTime.ToUnixTimeSeconds();
            }
            else
            {
                throw new ArgumentException("Invalid date.");
            }

            string? notes = PasswordNotes.Text;
            bool isViolated = Violated.IsChecked ?? false;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Password newPassword = new(id, category, account, timestamp, username, value, notes, isViolated, isLocked);

            DB.Instance.Passwords.Update(newPassword);
        }
    }
}
