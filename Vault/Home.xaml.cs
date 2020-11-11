using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vault.CustomControls;
using Vault.Core;
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Finestra principale.
    /// </summary>
    public partial class Home : Window
    {
        private bool loaded = false;
        private int loadedSection = 0;


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
            switch (Settings.Default.SectionToLoad)
            {
                case 1:
                    LoadPasswordSection();
                    break;
                case 2:
                    LoadPasswordSection();
                    break;
                case 3:
                    LoadPasswordSection();
                    break;
                default:
                    break;
            }
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
            if (new DialogWindow(new CardWindow(null)).Show() == CardWindow.EDIT)
            {
                if (Search.Text != "") LoadSearchedCards();
                else LoadAllCards();
            }
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new NoteWindow(null)).Show() == NoteWindow.EDIT)
            {
                if (Search.Text != "") LoadSearchedNotes();
                else LoadAllNotes();
            }
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
            Card selectedCard = VaultDB.Instance.Cards.GetRecord(((CardPreview)sender).ID);
            if (!selectedCard.RequestKey)
            {
                if (new DialogWindow(new CardWindow(selectedCard)).Show() == CardWindow.EDIT)
                {
                    if (Search.Text != "") LoadSearchedCards();
                    else LoadAllCards();
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show() == KeyWindow.CONFIRMED)
                {
                    if (new DialogWindow(new CardWindow(selectedCard)).Show() == CardWindow.EDIT)
                    {
                        if (Search.Text != "") LoadSearchedCards();
                        else LoadAllCards();
                    }
                }
            }
        }

        private void NotePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Note selectedNote = VaultDB.Instance.Notes.GetRecord(((NotePreview)sender).ID);
            if (!selectedNote.RequestKey)
            {
                if (new DialogWindow(new NoteWindow(selectedNote)).Show() == NoteWindow.EDIT)
                {
                    if (Search.Text != "") LoadSearchedNotes();
                    else LoadAllNotes();
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show() == KeyWindow.CONFIRMED)
                {
                    if (new DialogWindow(new NoteWindow(selectedNote)).Show() == NoteWindow.EDIT)
                    {
                        if (Search.Text != "") LoadSearchedNotes();
                        else LoadAllNotes();
                    }
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (loadedSection)
            {
                case 1:
                    if (Search.Text != "") LoadSearchedPasswords();
                    else LoadAllPasswords();
                    break;
                case 2:
                    if (Search.Text != "") LoadSearchedCards();
                    else LoadAllCards();
                    break;
                case 3:
                    if (Search.Text != "") LoadSearchedNotes();
                    else LoadAllNotes();
                    break;
                default:
                    break;
            }
        }

        private void LoadSearchedPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Search.Text, Global.Instance.UserID));

        private void LoadAllPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Global.Instance.UserID));

        private void LoadPasswords(List<Password> passwords) => PasswordList.ItemsSource = passwords;

        private void LoadSearchedCards() => LoadCards(VaultDB.Instance.Cards.GetRecords(Search.Text, Global.Instance.UserID));

        private void LoadAllCards() => LoadCards(VaultDB.Instance.Cards.GetRecords(Global.Instance.UserID));

        private void LoadCards(List<Card> cards) => CardList.ItemsSource = cards;

        private void LoadSearchedNotes() => LoadNotes(VaultDB.Instance.Notes.GetRecords(Search.Text, Global.Instance.UserID));

        private void LoadAllNotes() => LoadNotes(VaultDB.Instance.Notes.GetRecords(Global.Instance.UserID));

        private void LoadNotes(List<Note> notes) => NoteList.ItemsSource = notes;

        #region Switcher

        private void SwitchToPasswordSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded && e.IsActivated) LoadPasswordSection();
        }

        private void SwitchToCardSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded && e.IsActivated) LoadCardSection();
        }

        private void SwitchToNoteSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded && e.IsActivated) LoadNoteSection();
        }

        private void LoadPasswordSection()
        {
            loadedSection = 1;
            SwitchToCardSection.IsActivated = false;
            SwitchToNoteSection.IsActivated = false;
            CardSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Collapsed;
            PasswordSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadAllPasswords();
        }

        private void LoadCardSection()
        {
            loadedSection = 2;
            SwitchToPasswordSection.IsActivated = false;
            SwitchToNoteSection.IsActivated = false;
            PasswordSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Collapsed;
            CardSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadAllCards();
        }

        private void LoadNoteSection()
        {
            loadedSection = 3;
            SwitchToCardSection.IsActivated = false;
            SwitchToPasswordSection.IsActivated = false;
            CardSection.Visibility = Visibility.Collapsed;
            PasswordSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadAllNotes();
        }

        #endregion
    }
}
