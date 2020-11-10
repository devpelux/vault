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

        private void NewPassword_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new PasswordWindow(null)).Show() == PasswordWindow.EDIT)
            {
                if (Search.Text != "") LoadSearchedPasswords();
                else LoadAllPasswords();
            }
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PasswordPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Password selectedPassword = VaultDB.Instance.Passwords.GetRecord(((PasswordPreview)sender).ID);
            if (!selectedPassword.RequestKey)
            {
                if (new DialogWindow(new PasswordWindow(selectedPassword)).Show() == PasswordWindow.EDIT)
                {
                    if (Search.Text != "") LoadSearchedPasswords();
                    else LoadAllPasswords();
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show() == KeyWindow.CONFIRMED)
                {
                    if (new DialogWindow(new PasswordWindow(selectedPassword)).Show() == PasswordWindow.EDIT)
                    {
                        if (Search.Text != "") LoadSearchedPasswords();
                        else LoadAllPasswords();
                    }
                }
            }
        }

        private void CardPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void NotePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search.Text != "") LoadSearchedPasswords();
            else LoadAllPasswords();
        }

        private void LoadSearchedPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Search.Text, Global.Instance.UserID));

        private void LoadAllPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Global.Instance.UserID));

        private void LoadPasswords(List<Password> elements) => PasswordList.ItemsSource = elements;

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
