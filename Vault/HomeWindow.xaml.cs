﻿using FullControls.Common;
using FullControls.Controls;
using FullControls.SystemComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vault.Core;
using Vault.Core.Controls;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using Vault.Core.Settings;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Home window.
    /// </summary>
    public partial class HomeWindow : AvalonWindow
    {
        private const int PASSWORD_SECTION = 0;
        private const int CARD_SECTION = 1;
        private const int NOTE_SECTION = 2;

        private List<Password>? passwords;
        private List<Card>? cards;
        private List<Note>? notes;

        private List<Category>? categories;

        private bool enableSwitch = true;
        private int loadedSection = -1;


        /// <summary>
        /// Initializes a new <see cref="HomeWindow"/>.
        /// </summary>
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TrayIcon.Instance.SetIconType(TrayIconType.Unlocked);
            TrayIcon.Instance.LogoutCommandExecuted += TrayIcon_LogoutClick;
            Reload(Settings.Instance.GetSetting("section_to_load", 0));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            TrayIcon.Instance.SetIconType(TrayIconType.Locked);
            TrayIcon.Instance.LogoutCommandExecuted -= TrayIcon_LogoutClick;
            App.RequestShutDown(this);
        }

        private void TrayIcon_LogoutClick(object? sender, EventArgs e) => Logout();

        private void LogoutButton_Click(object sender, RoutedEventArgs e) => Logout();

        /// <summary>
        /// Executes the logout by terminating the current session and loading the credentials window.
        /// </summary>
        private void Logout()
        {
            //Terminates the current session.
            App.TerminateSession();

            //Loads the credentials window.
            new CredentialsWindow(CredentialsWindow.Request.Login).Show();
            Close();
        }

        private void Reload(int sectionToLoad)
        {
            categories = DB.Instance.Categories.GetAll();
            LoadSection(sectionToLoad);
            SelectSwitch(sectionToLoad);
        }

        private void ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }

        private void EditCategories_Click(object sender, RoutedEventArgs e)
        {
            _ = new DialogWindow(new CategoriesWindow()).Show();
            Reload(loadedSection);
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            new ReportWindow().ShowDialog();
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
            if (new DialogWindow(new PasswordWindow(null)).Show() != null)
            {
                LoadPasswords(Search.Text);
            }
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new CardWindow(null)).Show() != null)
            {
                LoadCards(Search.Text);
            }
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            if (new DialogWindow(new NoteWindow(null)).Show() != null)
            {
                LoadNotes(Search.Text);
            }
        }

        #endregion

        #region Click on element previews

        private void PasswordPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Password? selectedPassword = DB.Instance.Passwords.Get((int)((DataItem)sender).Tag);

            if (selectedPassword == null) return;

            if (!selectedPassword.IsLocked)
            {
                if (new DialogWindow(new PasswordWindow(selectedPassword)).Show() != null)
                {
                    LoadPasswords(Search.Text);
                }
            }
            else
            {
                if ((bool?)new DialogWindow(new CredentialsWindow(CredentialsWindow.Request.Reauthentication)).Show() == true)
                {
                    if (new DialogWindow(new PasswordWindow(selectedPassword)).Show() != null)
                    {
                        LoadPasswords(Search.Text);
                    }
                }
            }
        }

        private void CardPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Card? selectedCard = DB.Instance.Cards.Get((int)((DataItem)sender).Tag);

            if (selectedCard == null) return;

            if (!selectedCard.IsLocked)
            {
                if (new DialogWindow(new CardWindow(selectedCard)).Show() != null)
                {
                    LoadCards(Search.Text);
                }
            }
            else
            {
                if ((bool?)new DialogWindow(new CredentialsWindow(CredentialsWindow.Request.Reauthentication)).Show() == true)
                {
                    if (new DialogWindow(new CardWindow(selectedCard)).Show() != null)
                    {
                        LoadCards(Search.Text);
                    }
                }
            }
        }

        private void NotePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Note? selectedNote = DB.Instance.Notes.Get((int)((DataItem)sender).Tag);

            if (selectedNote == null) return;

            if (!selectedNote.IsLocked)
            {
                if (new DialogWindow(new NoteWindow(selectedNote)).Show() != null)
                {
                    LoadNotes(Search.Text);
                }
            }
            else
            {
                if ((bool?)new DialogWindow(new CredentialsWindow(CredentialsWindow.Request.Reauthentication)).Show() == true)
                {
                    if (new DialogWindow(new NoteWindow(selectedNote)).Show() != null)
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

        private AccordionItemCollection GenerateAccordionItems<T>(List<T> datas, List<Category>? categories) where T : Data
        {
            AccordionItemCollection accordionItems = new();

            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    List<T> datasByCategory = datas.FindAll(data => data.Category == category.Name);
                    List<DataItemAdapter> items = datasByCategory.ConvertAll(data => new DataItemAdapter(data));

                    if (items.Count > 0)
                    {
                        items[0].Position = DataItemAdapter.ItemPosition.First;
                        items[^1].Position = DataItemAdapter.ItemPosition.Last;

                        ItemsControlAccordionItem item = new()
                        {
                            Style = FindResource("DarkItemsControlAccordionItem") as Style,
                            Margin = new Thickness(0, 2, 0, 2),
                            Header = Utility.AdaptLabel(category),
                            IsExpanded = category.IsExpanded,
                            ItemsSource = items,
                            ItemTemplate = GetListTemplate(datas.GetType()),
                            Tag = category.Name
                        };

                        accordionItems.Add(item);
                    }
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
