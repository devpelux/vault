using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vault.CustomControls;
using Vault.Core;
using FullControls;
using System.ComponentModel;
using System.Linq;
using System;

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

        private static int UserID => Session.Instance.UserID;
        private static byte[] Key => Session.Instance.Key;

        private List<Password> passwords;
        private List<Card> cards;
        private List<Note> notes;

        private List<Category> categories;

        private bool enableSwitch = true;
        private int loadedSection = -1;
        private bool minimizeInTrayOnClose = true;

        private const StringComparison SearchType = StringComparison.CurrentCultureIgnoreCase;


        public Home()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload(SettingsWrapper.SectionToLoad);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Session.Instance.Dispose();
        }

        private void Window_CloseAction(object sender, ActionEventArgs e)
        {
            if (minimizeInTrayOnClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Window_MinimizeAction(object sender, ActionEventArgs e)
        {
            if (minimizeInTrayOnClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        #region NotifyIcon

        private void CMShowHome_Click(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void CMLogout_Click(object sender, RoutedEventArgs e)
        {
            minimizeInTrayOnClose = false;
            notifyIcon.Dispose();
            new LoginWindow().Show();
            Close();
        }

        private void CMLogoutAndClose_Click(object sender, RoutedEventArgs e)
        {
            minimizeInTrayOnClose = false;
            notifyIcon.Dispose();
            Close();
        }

        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
        }

        #endregion

        private void Reload(int sectionToLoad)
        {
            categories = VaultDB.Instance.Categories.GetRecords(UserID);
            LoadSection(sectionToLoad);
            SelectSwitch(sectionToLoad);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            minimizeInTrayOnClose = false;
            notifyIcon.Dispose();
            new LoginWindow().Show();
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().Show();
        }

        private void EditCategories_Click(object sender, RoutedEventArgs e)
        {
            _ = new DialogWindow(new CategoriesWindow()).Show();
            Reload(loadedSection);
        }

        private void Lists_ItemExpandedChanged(object sender, ItemExpandedEventArgs e)
        {
            Category editedCategory = categories[e.Index] with { IsExpanded = e.IsExpanded };
            categories[e.Index] = editedCategory;
            VaultDB.Instance.Categories.UpdateRecord(editedCategory);
        }

        #region New element buttons

        private void NewPassword_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new PasswordWindow(null, categories)).Show().Equals(PasswordWindow.EDIT))
            {
                LoadPasswords(Search.Text);
            }
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new CardWindow(null, categories)).Show().Equals(CardWindow.EDIT))
            {
                LoadCards(Search.Text);
            }
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new NoteWindow(null, categories)).Show().Equals(NoteWindow.EDIT))
            {
                LoadNotes(Search.Text);
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
                    LoadPasswords(Search.Text);
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show().Equals(KeyWindow.CONFIRMED))
                {
                    if (new DialogWindow(new PasswordWindow(selectedPassword, categories)).Show().Equals(PasswordWindow.EDIT))
                    {
                        LoadPasswords(Search.Text);
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
                    LoadCards(Search.Text);
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show().Equals(KeyWindow.CONFIRMED))
                {
                    if (new DialogWindow(new CardWindow(selectedCard, categories)).Show().Equals(CardWindow.EDIT))
                    {
                        LoadCards(Search.Text);
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
                    LoadNotes(Search.Text);
                }
            }
            else
            {
                if (new DialogWindow(new KeyWindow()).Show().Equals(KeyWindow.CONFIRMED))
                {
                    if (new DialogWindow(new NoteWindow(selectedNote, categories)).Show().Equals(NoteWindow.EDIT))
                    {
                        LoadNotes(Search.Text);
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
                    LoadPasswords(Search.Text);
                    break;
                case CARD_SECTION:
                    LoadCards(Search.Text);
                    break;
                case NOTE_SECTION:
                    LoadNotes(Search.Text);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Loaders

        private void LoadPasswords(string label = "")
        {
            passwords = label is not null and not ""
                ? PreDecryptAll(VaultDB.Instance.Passwords.GetRecords(UserID), Key).FindAll(p => p.Label.Contains(label, SearchType))
                : PreDecryptAll(VaultDB.Instance.Passwords.GetRecords(UserID), Key);
            PasswordList.ItemsSource = BuildVaultTreeViewItemSource(passwords, categories);
        }

        private void LoadCards(string label = "")
        {
            cards = label is not null and not ""
                ? PreDecryptAll(VaultDB.Instance.Cards.GetRecords(UserID), Key).FindAll(c => c.Label.Contains(label, SearchType))
                : PreDecryptAll(VaultDB.Instance.Cards.GetRecords(UserID), Key);
            CardList.ItemsSource = BuildVaultTreeViewItemSource(cards, categories);
        }

        private void LoadNotes(string title = "")
        {
            notes = title is not null and not ""
                ? PreDecryptAll(VaultDB.Instance.Notes.GetRecords(UserID), Key).FindAll(n => n.Title.Contains(title, SearchType))
                : PreDecryptAll(VaultDB.Instance.Notes.GetRecords(UserID), Key);
            NoteList.ItemsSource = BuildVaultTreeViewItemSource(notes, categories);
        }

        private static List<T> PreDecryptAll<T>(List<T> list, byte[] key) where T : IDecryptable<T> => list.Select(d => d.PreDecrypt(key)).ToList();

        private static List<VaultTreeViewItemSource> BuildVaultTreeViewItemSource<T>(List<T> list, List<Category> categories) where T : ICategorizable
            => categories.Select(category =>
            {
                List<T> filteredElements = list.FindAll(e => e.Category == category.ID);
                return new VaultTreeViewItemSource(category.Label, category.IsExpanded, filteredElements);
            }).ToList();

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
            LoadPasswords();
            loadedSection = PASSWORD_SECTION;
        }

        private void LoadCardSection()
        {
            loadedSection = -1;
            PasswordSection.Visibility = Visibility.Collapsed;
            CardSection.Visibility = Visibility.Visible;
            NoteSection.Visibility = Visibility.Collapsed;
            Search.Text = "";
            LoadCards();
            loadedSection = CARD_SECTION;
        }

        private void LoadNoteSection()
        {
            loadedSection = -1;
            PasswordSection.Visibility = Visibility.Collapsed;
            CardSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Visible;
            Search.Text = "";
            LoadNotes();
            loadedSection = NOTE_SECTION;
        }

        #endregion
    }
}
