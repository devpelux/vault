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
        private const int PASSWORD_SECTION = 0;
        private const int CARD_SECTION = 1;
        private const int NOTE_SECTION = 2;

        private bool enableSwitch = true;
        private int loadedSection = -1;
        private List<Category> categories = null;


        public Home()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload(Settings.Default.SectionToLoad);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Session.Instance.Dispose();
        }

        private void Reload(int sectionToLoad)
        {
            categories = VaultDB.Instance.Categories.GetRecords(Session.Instance.UserID);
            LoadSection(sectionToLoad);
            SelectSwitch(sectionToLoad);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void EditCategories_Click(object sender, RoutedEventArgs e)
        {
            _ = new DialogWindow(new CategoriesWindow()).Show();
            Reload(0);
        }

        #region New element buttons

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

        #endregion

        #region Click on element previews

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

        #endregion

        #region Search

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (loadedSection)
            {
                case PASSWORD_SECTION:
                    if (Search.Text != "") LoadSearchedPasswords();
                    else LoadAllPasswords();
                    break;
                case CARD_SECTION:
                    if (Search.Text != "") LoadSearchedCards();
                    else LoadAllCards();
                    break;
                case NOTE_SECTION:
                    if (Search.Text != "") LoadSearchedNotes();
                    else LoadAllNotes();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Loaders

        private void LoadSearchedPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Search.Text, Session.Instance.UserID));

        private void LoadAllPasswords() => LoadPasswords(VaultDB.Instance.Passwords.GetRecords(Session.Instance.UserID));

        private void LoadSearchedCards() => LoadCards(VaultDB.Instance.Cards.GetRecords(Search.Text, Session.Instance.UserID));

        private void LoadAllCards() => LoadCards(VaultDB.Instance.Cards.GetRecords(Session.Instance.UserID));

        private void LoadSearchedNotes() => LoadNotes(VaultDB.Instance.Notes.GetRecords(Search.Text, Session.Instance.UserID));

        private void LoadAllNotes() => LoadNotes(VaultDB.Instance.Notes.GetRecords(Session.Instance.UserID));

        private void LoadPasswords(List<Password> passwords) => PasswordList.ItemsSource = Passwords.GroupByCategories(Passwords.DecryptForPreview(passwords, Session.Instance.Key), categories);

        private void LoadCards(List<Card> cards) => CardList.ItemsSource = Cards.GroupByCategories(Cards.DecryptForPreview(cards, Session.Instance.Key), categories);

        private void LoadNotes(List<Note> notes) => NoteList.ItemsSource = Notes.GroupByCategories(Notes.DecryptForPreview(notes, Session.Instance.Key), categories);

        #endregion

        #region Switcher

        private void SwitchSelected(object sender, RoutedEventArgs e)
        {
            if (!enableSwitch) return;
            if (SwitchToPasswordSection.IsChecked == true)
            {
                LoadSection(PASSWORD_SECTION);
            }
            else if (SwitchToCardSection.IsChecked == true)
            {
                LoadSection(CARD_SECTION);
            }
            else if (SwitchToNoteSection.IsChecked == true)
            {
                LoadSection(NOTE_SECTION);
            }
        }

        private void SelectSwitch(int section)
        {
            enableSwitch = false;
            switch (section)
            {
                case PASSWORD_SECTION:
                    SwitchToPasswordSection.IsChecked = true;
                    break;
                case CARD_SECTION:
                    SwitchToCardSection.IsChecked = true;
                    break;
                case NOTE_SECTION:
                    SwitchToNoteSection.IsChecked = true;
                    break;
                default:
                    break;
            }
            enableSwitch = true;
        }

        private void LoadSection(int section)
        {
            switch (section)
            {
                case PASSWORD_SECTION:
                    LoadPasswordSection();
                    break;
                case CARD_SECTION:
                    LoadCardSection();
                    break;
                case NOTE_SECTION:
                    LoadNoteSection();
                    break;
                default:
                    break;
            }
        }

        private void LoadPasswordSection()
        {
            loadedSection = -1;
            PasswordSection.Visibility = Visibility.Visible;
            CardSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Collapsed;
            Search.Text = "";
            LoadAllPasswords();
            loadedSection = PASSWORD_SECTION;
        }

        private void LoadCardSection()
        {
            loadedSection = -1;
            PasswordSection.Visibility = Visibility.Collapsed;
            CardSection.Visibility = Visibility.Visible;
            NoteSection.Visibility = Visibility.Collapsed;
            Search.Text = "";
            LoadAllCards();
            loadedSection = CARD_SECTION;
        }

        private void LoadNoteSection()
        {
            loadedSection = -1;
            PasswordSection.Visibility = Visibility.Collapsed;
            CardSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadAllNotes();
            loadedSection = NOTE_SECTION;
        }

        #endregion
    }
}
