using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vault.CustomControls;
using Vault.Core;
using Vault.Properties;
using FullControls;
using System.ComponentModel;

namespace Vault
{
    /// <summary>
    /// Finestra principale.
    /// </summary>
    public partial class Home : EWindow
    {
        private bool loaded = false;
        private int loadedSection = 0;
        private List<Category> categories = null;


        public Home()
        {
            InitializeComponent();
        }

        private void EWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Reload();
        }

        private void EWindow_Closing(object sender, CancelEventArgs e)
        {
            Session.Instance.Dispose();
        }

        private void Reload()
        {
            categories = VaultDB.Instance.Categories.GetRecords(Session.Instance.UserID);
            switch (Settings.Default.SectionToLoad)
            {
                case 1:
                    LoadPasswordSection();
                    break;
                case 2:
                    LoadCardSection();
                    break;
                case 3:
                    LoadNoteSection();
                    break;
                default:
                    break;
            }
            loaded = true;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void EditCategories_Click(object sender, RoutedEventArgs e)
        {
            _ = new DialogWindow(new CategoriesWindow()).Show();
            Reload();
        }

        private void NewPassword_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new PasswordWindow(null, categories)).Show().Equals(PasswordWindow.EDIT))
            {
                if (Search.Text != "") LoadSearchedPasswords();
                else LoadAllPasswords();
            }
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new CardWindow(null, categories)).Show().Equals(CardWindow.EDIT))
            {
                if (Search.Text != "") LoadSearchedCards();
                else LoadAllCards();
            }
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new NoteWindow(null, categories)).Show().Equals(NoteWindow.EDIT))
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
                if (new DialogWindow(new PasswordWindow(selectedPassword, categories)).Show().Equals(PasswordWindow.EDIT))
                {
                    if (Search.Text != "") LoadSearchedPasswords();
                    else LoadAllPasswords();
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show().Equals(KeyWindow.CONFIRMED))
                {
                    if (new DialogWindow(new PasswordWindow(selectedPassword, categories)).Show().Equals(PasswordWindow.EDIT))
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
                if (new DialogWindow(new CardWindow(selectedCard, categories)).Show().Equals(CardWindow.EDIT))
                {
                    if (Search.Text != "") LoadSearchedCards();
                    else LoadAllCards();
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show().Equals(KeyWindow.CONFIRMED))
                {
                    if (new DialogWindow(new CardWindow(selectedCard, categories)).Show().Equals(CardWindow.EDIT))
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
                if (new DialogWindow(new NoteWindow(selectedNote, categories)).Show().Equals(NoteWindow.EDIT))
                {
                    if (Search.Text != "") LoadSearchedNotes();
                    else LoadAllNotes();
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show().Equals(KeyWindow.CONFIRMED))
                {
                    if (new DialogWindow(new NoteWindow(selectedNote, categories)).Show().Equals(NoteWindow.EDIT))
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

        private void LoadSearchedPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Search.Text, Session.Instance.UserID));

        private void LoadAllPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Session.Instance.UserID));

        private void LoadPasswords(List<Password> passwords) => PasswordList.ItemsSource = Passwords.GroupByCategories(Passwords.DecryptForPreview(passwords, Session.Instance.Key), categories);

        private void LoadSearchedCards() => LoadCards(VaultDB.Instance.Cards.GetRecords(Search.Text, Session.Instance.UserID));

        private void LoadAllCards() => LoadCards(VaultDB.Instance.Cards.GetRecords(Session.Instance.UserID));

        private void LoadCards(List<Card> cards) => CardList.ItemsSource = Cards.GroupByCategories(Cards.DecryptForPreview(cards, Session.Instance.Key), categories);

        private void LoadSearchedNotes() => LoadNotes(VaultDB.Instance.Notes.GetRecords(Search.Text, Session.Instance.UserID));

        private void LoadAllNotes() => LoadNotes(VaultDB.Instance.Notes.GetRecords(Session.Instance.UserID));

        private void LoadNotes(List<Note> notes) => NoteList.ItemsSource = Notes.GroupByCategories(Notes.DecryptForPreview(notes, Session.Instance.Key), categories);

        #region Switcher

        private void SwitchToPasswordSection_ActivationChanged(object sender, RoutedEventArgs e)
        {
            if (loaded) LoadPasswordSection();
        }

        private void SwitchToCardSection_ActivationChanged(object sender, RoutedEventArgs e)
        {
            if (loaded) LoadCardSection();
        }

        private void SwitchToNoteSection_ActivationChanged(object sender, RoutedEventArgs e)
        {
            if (loaded) LoadNoteSection();
        }

        private void LoadPasswordSection()
        {
            loadedSection = 1;
            SwitchToPasswordSection.IsChecked = true;
            SwitchToCardSection.IsChecked = false;
            SwitchToNoteSection.IsChecked = false;
            CardSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Collapsed;
            PasswordSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadAllPasswords();
        }

        private void LoadCardSection()
        {
            loadedSection = 2;
            SwitchToCardSection.IsChecked = true;
            SwitchToPasswordSection.IsChecked = false;
            SwitchToNoteSection.IsChecked = false;
            PasswordSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Collapsed;
            CardSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadAllCards();
        }

        private void LoadNoteSection()
        {
            loadedSection = 3;
            SwitchToNoteSection.IsChecked = true;
            SwitchToCardSection.IsChecked = false;
            SwitchToPasswordSection.IsChecked = false;
            CardSection.Visibility = Visibility.Collapsed;
            PasswordSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadAllNotes();
        }

        #endregion
    }
}
