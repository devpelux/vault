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
    public partial class Home : Window, IDialogListener
    {
        private bool loaded = false;
        private Password selectedPassword = null;


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
            new PasswordWindow(this, null).ShowDialog();
        }

        private void ElementPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedPassword = VaultDB.Instance.Passwords.GetRecord(((ItemElementPreview)sender).ID);
            if (!selectedPassword.RequestKey)
            {
                new PasswordWindow(this, selectedPassword).ShowDialog();
                selectedPassword = null;
            }
            else
            {
                new KeyWindow(this).ShowDialog();
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search.Text != "") LoadSearchedPasswords();
            else LoadAllPasswords();
        }

        public void OnDialogAction(DialogAction action, string actionType)
        {
            if (action == DialogAction.ACTION)
            {
                switch (actionType)
                {
                    case PasswordWindow.DONE:
                        if (Search.Text != "") LoadSearchedPasswords();
                        else LoadAllPasswords();
                        break;
                    case KeyWindow.CONFIRMED:
                        new PasswordWindow(this, selectedPassword).ShowDialog();
                        selectedPassword = null;
                        break;
                    case KeyWindow.UNCONFIRMED:
                        selectedPassword = null;
                        break;
                    default:
                        break;
                }
            }
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
