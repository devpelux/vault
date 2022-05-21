using CoreTools.Extensions;
using FullControls.SystemComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using Vault.Core;
using Vault.Core.Controls;
using Vault.Core.Database;
using Vault.Core.Database.Data;
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
                new MessageWindow("Data non valida!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
            }
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the password if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (password == null) return;

            DB.Instance.Passwords.Remove(password.Id);

            Result = "deleted";
            Close();
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
        private void AddPassword()
        {
            string category = categories[PasswordCategory.SelectedIndex].Name;
            string account = PasswordAccount.Text;
            string username = PasswordUsername.Text;
            string value = PasswordValue.Password;

            int year = PasswordTimestampYear.Text.ToInt(1);
            int month = PasswordTimestampMonth.Text.ToInt(1);
            int day = PasswordTimestampDay.Text.ToInt(1);
            DateTimeOffset localTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), Math.Max(day, 1), 0, 0, 0, DateTimeKind.Local);

            long timestamp = localTime.ToUnixTimeSeconds();

            string? notes = PasswordNotes.Text;
            bool isViolated = Violated.IsChecked ?? false;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Password newPassword = new(category, account, timestamp, username, value, notes, isViolated, isLocked);

            DB.Instance.Passwords.Add(newPassword);
        }

        /// <summary>
        /// Edits the password.
        /// </summary>
        private void EditPassword()
        {
            if (password == null) return;

            int id = password.Id;

            string category = categories[PasswordCategory.SelectedIndex].Name;
            string account = PasswordAccount.Text;
            string username = PasswordUsername.Text;
            string value = PasswordValue.Password;

            int year = PasswordTimestampYear.Text.ToInt(1);
            int month = PasswordTimestampMonth.Text.ToInt(1);
            int day = PasswordTimestampDay.Text.ToInt(1);
            DateTimeOffset localTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), Math.Max(day, 1), 0, 0, 0, DateTimeKind.Local);

            long timestamp = localTime.ToUnixTimeSeconds();

            string? notes = PasswordNotes.Text;
            bool isViolated = Violated.IsChecked ?? false;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Password newPassword = new(id, category, account, timestamp, username, value, notes, isViolated, isLocked);

            DB.Instance.Passwords.Update(newPassword);
        }
    }
}
