﻿using System.Windows;
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
        private readonly Password password;

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
                PasswordName.Text = password.Name;
                PasswordDescription.Text = password.Description;
                PasswordRequestKey.IsChecked = password.RequestKey;
                PasswordWebsite.Text = password.Website;
                PasswordUsername.Text = password.Username;
                PasswordKey.SetPassword(password.Key);
                PasswordNote.Text = password.Note;
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
            Password password = new Password
                (
                    Passwords.NewID,
                    Global.Instance.UserID,
                    PasswordName.Text,
                    "Generic",
                    PasswordDescription.Text,
                    PasswordRequestKey.IsChecked ?? false,
                    PasswordWebsite.Text,
                    PasswordUsername.Text,
                    PasswordKey.GetPassword(),
                    PasswordNote.Text
                );
            VaultDB.Instance.Passwords.AddRecord(Passwords.Encrypt(password, Global.Instance.Key));
        }

        private void EditElement()
        {
            Password editedPassword = password with
            {
                Name = PasswordName.Text,
                Category = "Generic",
                Description = PasswordDescription.Text,
                RequestKey = PasswordRequestKey.IsChecked ?? false,
                Website = PasswordWebsite.Text,
                Username = PasswordUsername.Text,
                Key = PasswordKey.GetPassword(),
                Note = PasswordNote.Text
            };
            VaultDB.Instance.Passwords.UpdateRecord(Passwords.Encrypt(editedPassword, Global.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Passwords.RemoveRecord(password.ID);
            Result = EDIT;
            Close();
        }
    }
}