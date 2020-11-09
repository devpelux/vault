using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vault.CustomControls;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra principale.
    /// </summary>
    public partial class Home : Window
    {
        private bool loaded = false;


        public Home()
        {
            InitializeComponent();
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            LoadAllPasswords();
        }

        private void NewElement_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow(null);
            _ = passwordWindow.ShowDialog();
            if (passwordWindow.Result == PasswordWindow.EDIT)
            {
                if (Search.Text != "") LoadSearchedPasswords();
                else LoadAllPasswords();
            }
        }

        private void ElementPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Password selectedPassword = VaultDB.Instance.Passwords.GetRecord(((ItemElementPreview)sender).ID);
            if (!selectedPassword.RequestKey)
            {
                PasswordWindow passwordWindow = new PasswordWindow(selectedPassword);
                _ = passwordWindow.ShowDialog();
                if (passwordWindow.Result == PasswordWindow.EDIT)
                {
                    if (Search.Text != "") LoadSearchedPasswords();
                    else LoadAllPasswords();
                }
            }
            else
            {
                KeyWindow keyWindow = new KeyWindow();
                _ = keyWindow.ShowDialog();
                if (keyWindow.Result == KeyWindow.CONFIRMED)
                {
                    PasswordWindow passwordWindow = new PasswordWindow(selectedPassword);
                    _ = passwordWindow.ShowDialog();
                    if (passwordWindow.Result == PasswordWindow.EDIT)
                    {
                        if (Search.Text != "") LoadSearchedPasswords();
                        else LoadAllPasswords();
                    }
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search.Text != "") LoadSearchedPasswords();
            else LoadAllPasswords();
        }

        private void LoadSearchedPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Search.Text, Global.Instance.UserID));

        private void LoadAllPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Global.Instance.UserID));

        private void LoadPasswords(List<Password> elements) => ListElements.ItemsSource = elements;

        #region Switcher

        private void SwitchToPasswordSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded && e.IsActivated)
            {
                SwitchToCardSection.IsActivated = false;
                SwitchToNoteSection.IsActivated = false;
                CardSection.Visibility = Visibility.Collapsed;
                NoteSection.Visibility = Visibility.Collapsed;
                PasswordSection.Visibility = Visibility.Visible;
            }
        }

        private void SwitchToCardSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded && e.IsActivated)
            {
                SwitchToPasswordSection.IsActivated = false;
                SwitchToNoteSection.IsActivated = false;
                PasswordSection.Visibility = Visibility.Collapsed;
                NoteSection.Visibility = Visibility.Collapsed;
                CardSection.Visibility = Visibility.Visible;
            }
        }

        private void SwitchToNoteSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded && e.IsActivated)
            {
                SwitchToCardSection.IsActivated = false;
                SwitchToPasswordSection.IsActivated = false;
                CardSection.Visibility = Visibility.Collapsed;
                PasswordSection.Visibility = Visibility.Collapsed;
                NoteSection.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
