using FullControls.Common;
using FullControls.Controls;
using FullControls.SystemComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vault.Controls;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using Vault.Core.Settings;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Home window.
    /// </summary>
    public partial class Home : AvalonWindow
    {
        private const int PASSWORD_SECTION = 0;
        private const int CARD_SECTION = 1;
        private const int NOTE_SECTION = 2;

        private List<Password> passwords;
        private List<Card> cards;
        private List<Note> notes;

        private List<Category> categories;

        private bool enableSwitch = true;
        private int loadedSection = -1;


        public Home()
        {
            InitializeComponent();
            TrayIcon.Instance.HomeWindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload(Settings.Instance.GetSetting("section_to_load", 0));
            TrayIcon.Instance.WindowToShow = null;
        }

        private void Window_CloseCommandExecuting(object sender, EventArgs e)
        {
            if (Settings.Instance.GetSetting("exit_explicit", true) == true) TrayIcon.Instance.WindowToShow = nameof(Home);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (TrayIcon.Instance.WindowToShow == null && Application.Current.Windows.Count == 0)
            {
                TrayIcon.Instance.VaultStatus = VaultStatus.Locked;
                Application.Current.Shutdown();
            }
            TrayIcon.Instance.HomeWindow = null;
        }

        private void Reload(int sectionToLoad)
        {
            categories = DB.Instance.Categories.GetAll();
            LoadSection(sectionToLoad);
            SelectSwitch(sectionToLoad);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            SessionSettings.Instance.Clear();
            new CredentialsWindow(CredentialsWindow.Request.Login).Show();
            TrayIcon.Instance.VaultStatus = VaultStatus.Locked;
            Close();
        }

        private void ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().Show();
        }

        private void EditCategories_Click(object sender, RoutedEventArgs e)
        {
            _ = new DialogWindow(new CategoriesWindow()).Show();
            Reload(loadedSection);
        }

        private void Lists_ItemIsExpandedChanged(object sender, ItemExpandedChangedEventArgs e)
        {
            string categoryName = (string)((Accordion)sender).Items[e.Index].Tag;
            Category? category = DB.Instance.Categories.Get(categoryName);
            if (category != null)
            {
                category.IsExpanded = e.IsExpanded;

                DB.Instance.Categories.Update(category);
                categories = DB.Instance.Categories.GetAll();
            }
        }

        #region New element buttons

        private void NewPassword_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new PasswordWindow(null)).Show()?.Equals("edit") == true)
            {
                LoadPasswords(Search.Text);
            }
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new CardWindow(null)).Show()?.Equals("edit") == true)
            {
                LoadCards(Search.Text);
            }
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new NoteWindow(null)).Show()?.Equals("edit") == true)
            {
                LoadNotes(Search.Text);
            }
        }

        #endregion

        #region Click on element previews

        private void PasswordPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Password? selectedPassword = DB.Instance.Passwords.Get(((PasswordPreview)sender).ID);

            if (selectedPassword == null) return;

            if (!selectedPassword.IsLocked)
            {
                if (new DialogWindow(new PasswordWindow(selectedPassword)).Show()?.Equals("edit") == true)
                {
                    LoadPasswords(Search.Text);
                }
            }
            else
            {
                if ((bool?)new DialogWindow(new CredentialsWindow(CredentialsWindow.Request.Reauthentication)).Show() == true)
                {
                    if (new DialogWindow(new PasswordWindow(selectedPassword)).Show()?.Equals("edit") == true)
                    {
                        LoadPasswords(Search.Text);
                    }
                }
            }
        }

        private void CardPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Card? selectedCard = DB.Instance.Cards.Get(((CardPreview)sender).ID);

            if (selectedCard == null) return;

            if (!selectedCard.IsLocked)
            {
                if (new DialogWindow(new CardWindow(selectedCard)).Show()?.Equals("edit") == true)
                {
                    LoadCards(Search.Text);
                }
            }
            else
            {
                if ((bool?)new DialogWindow(new CredentialsWindow(CredentialsWindow.Request.Reauthentication)).Show() == true)
                {
                    if (new DialogWindow(new CardWindow(selectedCard)).Show()?.Equals("edit") == true)
                    {
                        LoadCards(Search.Text);
                    }
                }
            }
        }

        private void NotePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Note? selectedNote = DB.Instance.Notes.Get(((NotePreview)sender).ID);

            if (selectedNote == null) return;

            if (!selectedNote.IsLocked)
            {
                if (new DialogWindow(new NoteWindow(selectedNote)).Show()?.Equals("edit") == true)
                {
                    LoadNotes(Search.Text);
                }
            }
            else
            {
                if ((bool?)new DialogWindow(new CredentialsWindow(CredentialsWindow.Request.Reauthentication)).Show() == true)
                {
                    if (new DialogWindow(new NoteWindow(selectedNote)).Show()?.Equals("edit") == true)
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

        private void LoadPasswords(string account = "")
        {
            passwords = DB.Instance.Passwords.GetAll().FindAll(password
                => account == string.Empty || password.Account.Contains(account, StringComparison.CurrentCultureIgnoreCase));
            PasswordList.Items = GenerateAccordionItems(passwords, categories);
        }

        private void LoadCards(string name = "")
        {
            cards = DB.Instance.Cards.GetAll().FindAll(card
                => name == string.Empty || card.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
            CardList.Items = GenerateAccordionItems(cards, categories);
        }

        private void LoadNotes(string title = "")
        {
            notes = DB.Instance.Notes.GetAll().FindAll(note
                => title == string.Empty || note.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase));
            NoteList.Items = GenerateAccordionItems(notes, categories);
        }

        private DataTemplate GetListTemplate(Type type)
        {
            if (type == typeof(List<Password>)) return (DataTemplate)PasswordList.FindResource("PasswordListDT");
            else if (type == typeof(List<Note>)) return (DataTemplate)NoteList.FindResource("NoteListDT");
            else return (DataTemplate)CardList.FindResource("CardListDT");
        }

        private AccordionItemCollection GenerateAccordionItems<T>(List<T> list, List<Category> categories) where T : Data
        {
            AccordionItemCollection accordionItems = new();

            foreach (Category cat in categories)
            {
                List<T> categoryFiltered = list.FindAll(e => e.Category == cat.Name);
                if (categoryFiltered.Count > 0)
                {
                    ItemsControlAccordionItem item = new()
                    {
                        Style = FindResource("DarkItemsControlAccordionItem") as Style,
                        Header = cat.Name,
                        IsExpanded = cat.IsExpanded,
                        ItemsSource = categoryFiltered,
                        ItemTemplate = GetListTemplate(list.GetType()),
                        Tag = cat.Name
                    };
                    accordionItems.Add(item);
                }
            }

            return accordionItems;
        }

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
