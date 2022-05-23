using FullControls.Common;
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
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Home window.
    /// </summary>
    public partial class HomeWindow : AvalonWindow
    {
        #region Sections identifiers

        private const long PASSWORDS_SECTION = 0;
        private const long NOTES_SECTION = 1;
        private const long CARDS_SECTION = 2;
        private const long DOCUMENTS_SECTION = 3;

        private long currentSection = -1;

        #endregion

        private bool lockSwitchers = true;

        /// <summary>
        /// Initializes a new <see cref="HomeWindow"/>.
        /// </summary>
        public HomeWindow()
        {
            InitializeComponent();
        }

        #region Login and logout

        /// <summary>
        /// Executed when the window is loaded.
        /// Changes the tray icon type to unlocked, add a listener for the logout by tray, reloads the window.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            TrayIcon.Instance.SetIconType(TrayIconType.Unlocked);
            TrayIcon.Instance.LogoutCommandExecuted += TrayIcon_LogoutClick;
            Reload();
        }

        /// <summary>
        /// Executed when the window is loaded.
        /// Removes the listener for the logout by tray, then requests to shut down the application.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            TrayIcon.Instance.LogoutCommandExecuted -= TrayIcon_LogoutClick;
            App.RequestShutDown(this);
        }

        /// <summary>
        /// Executed when is received the logout command from the tray icon.
        /// Executes the logout.
        /// </summary>
        private void TrayIcon_LogoutClick(object? sender, EventArgs e) => Logout();

        /// <summary>
        /// Executed when is pressed the logout button.
        /// Executes the logout.
        /// </summary>
        private void ExecuteLogout_Click(object sender, RoutedEventArgs e) => Logout();

        /// <summary>
        /// Changes the tray icon type to locked.
        /// Terminates the current session and loads the credentials window, then closes the window.
        /// </summary>
        private void Logout()
        {
            //Sets the icon to the locked icon.
            TrayIcon.Instance.SetIconType(TrayIconType.Locked);

            //Terminates the current session.
            App.TerminateSession();

            //Loads the credentials window.
            new CredentialsWindow(CredentialsWindow.Request.Login).Show();
            Close();
        }

        #endregion

        #region Categories, report and settings

        /// <summary>
        /// Executed when is pressed the categories button.
        /// Displays the categories window and, if it edits some categories, reloads.
        /// </summary>
        private void ShowCategories_Click(object sender, RoutedEventArgs e)
        {
            if (new CategoriesWindow() { Owner = this }.ShowDialogForResult<bool>())
            {
                Reload();
            }
        }

        /// <summary>
        /// Executed when is pressed the settings button.
        /// Displays the settings window.
        /// </summary>
        private void ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow() { Owner = this }.ShowDialog();
        }

        /// <summary>
        /// Executed when is pressed the report button.
        /// Displays the report window.
        /// </summary>
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            new ReportWindow() { Owner = this }.ShowDialog();
        }

        #endregion

        #region Section loading

        /// <summary>
        /// Reloads the window.
        /// If no section is loaded, loads the default section.
        /// </summary>
        private void Reload()
        {
            if (currentSection == -1)
            {
                //Gets the saved section from the settings
                string? user = InstanceSettings.Instance.GetSetting("username", "default");
                long defaultSection = Settings.Instance.GetSetting($"{user}_loaded_section", PASSWORDS_SECTION);
                LoadSection(defaultSection);
            }
            else LoadSection(currentSection);
        }

        /// <summary>
        /// Loads the specified section and saves remembers it into the settings.
        /// </summary>
        private void LoadSection(long section)
        {
            currentSection = section;

            //Saves the current section to remember it
            string? user = InstanceSettings.Instance.GetSetting("username", "default");
            Settings.Instance.SetSetting($"{user}_loaded_section", section);

            ReloadCurrentSection();
        }

        /// <summary>
        /// Reloads the current section.
        /// </summary>
        private void ReloadCurrentSection()
        {
            lockSwitchers = true;
            switch (currentSection)
            {
                case PASSWORDS_SECTION:
                    ShowPasswords.IsChecked = true;
                    DisplayFilteredDatas(DB.Instance.Passwords.GetAll(), Search.Text);
                    SectionName.Text = Strings.Passwords;
                    break;
                case NOTES_SECTION:
                    ShowNotes.IsChecked = true;
                    DisplayFilteredDatas(DB.Instance.Notes.GetAll(), Search.Text);
                    SectionName.Text = Strings.Notes;
                    break;
                case CARDS_SECTION:
                    ShowCards.IsChecked = true;
                    DisplayFilteredDatas(DB.Instance.Cards.GetAll(), Search.Text);
                    SectionName.Text = Strings.Cards;
                    break;
                case DOCUMENTS_SECTION:
                    ShowDocuments.IsChecked = true;
                    DisplayFilteredDatas(DB.Instance.Documents.GetAll(), Search.Text);
                    SectionName.Text = Strings.Documents;
                    break;
                default:
                    break;
            }
            lockSwitchers = false;
        }

        /// <summary>
        /// Executed when a switcher is clicked.
        /// Loads its relative section saved in the tag.
        /// </summary>
        private void Navigation_Switch(object sender, RoutedEventArgs e)
        {
            if (lockSwitchers) return;

            long section = (long)((Switcher)sender).Tag;
            LoadSection(section);
        }

        #endregion

        #region Add button and search

        /// <summary>
        /// Executed when is pressed the add button.
        /// Displays the edit window for the loaded section and, if it edits, reloads.
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string? result = null;
            switch (currentSection)
            {
                case PASSWORDS_SECTION:
                    result = new PasswordWindow(null) { Owner = this }.ShowDialogForResult<string>();
                    break;
                case NOTES_SECTION:
                    result = new NoteWindow(null) { Owner = this }.ShowDialogForResult<string>();
                    break;
                case CARDS_SECTION:
                    result = new CardWindow(null) { Owner = this }.ShowDialogForResult<string>();
                    break;
                case DOCUMENTS_SECTION:
                    result = new DocumentWindow(null) { Owner = this }.ShowDialogForResult<string>();
                    break;
                default:
                    break;
            }

            if (result != null) ReloadCurrentSection();
        }

        /// <summary>
        /// Executed when the search text is changed.
        /// Reloads the current section with the filtered elements.
        /// </summary>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadCurrentSection();
        }

        #endregion

        #region Items displayer

        /// <summary>
        /// Displays the datas after filtering them with a search.
        /// </summary>
        private void DisplayFilteredDatas<T>(List<T> datas, string filter = "") where T : Data
        {
            //Creates the accordion items collection for displaying the items
            AccordionItemCollection accordionItems = new();

            //Filter the items if the filter is not empty
            if (filter != "") datas = datas.FindAll(p => p.Header.Contains(filter, StringComparison.CurrentCultureIgnoreCase));

            //Creates an accordion item for each category
            List<Category> categories = DB.Instance.Categories.GetAll();
            foreach (Category category in categories)
            {
                //Filter the items for the category
                List<T> datasByCategory = datas.FindAll(data => data.Category == category.Name);

                //If the category does not have items, is not displayed
                if (datasByCategory.Count > 0)
                {
                    //Wraps each item into an adapter with extra info for displaying it correctly
                    List<DataItemAdapter> items = datasByCategory.ConvertAll(data => new DataItemAdapter(data));

                    //Expecially save the position of the first and last element
                    items[0].Position = DataItemAdapter.ItemPosition.First;
                    items[^1].Position = DataItemAdapter.ItemPosition.Last;

                    //Creates the accordion items and adds it to the list
                    ItemsControlAccordionItem item = new()
                    {
                        Style = FindResource("DarkAccordionItem") as Style,
                        Header = Utility.FormatCategoryLabel(category),
                        IsExpanded = category.IsExpanded,
                        ItemsSource = items,
                        ItemTemplate = (DataTemplate)SectionList.FindResource("SectionListItemTemplate"),
                        Tag = category.Name
                    };

                    accordionItems.Add(item);
                }
            }

            //Displays the accordion items
            SectionList.Items = accordionItems;
        }

        #endregion

        #region Accordion events

        /// <summary>
        /// Executed when an accordion item is expanded or collapsed.
        /// Updates the category with the expanding state.
        /// </summary>
        private void SectionList_ItemIsExpandedChanged(object sender, ItemExpandedChangedEventArgs e)
        {
            //Gets the category of the item
            string categoryName = (string)((Accordion)sender).Items[e.Index].Tag;
            Category? category = DB.Instance.Categories.Get(categoryName);

            if (category != null)
            {
                //Updates the expanding state of the category
                category.IsExpanded = e.IsExpanded;
                DB.Instance.Categories.Update(category);
            }
        }

        /// <summary>
        /// Executed when an item in an accordion item is selected.
        /// Displays the window for viewing the item info and editing the item.
        /// If the item is edited, reloads.
        /// </summary>
        private void DataItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Check if the item is locked
            bool isLocked = ((DataItem)sender).IsLocked;
            if (isLocked)
            {
                //If the item is locked, shows the credentials window to reauthenticate
                //If the reauthentication is succesful, the item will be unlocked
                isLocked = !new CredentialsWindow(CredentialsWindow.Request.Reauthentication) { Owner = this }.ShowDialogForResult<bool>();
            }

            //If the item is now unlocked, displays the window for viewing the item info and editing the item
            if (!isLocked)
            {
                string? result = null;

                switch (currentSection)
                {
                    case PASSWORDS_SECTION:
                        Password? password = DB.Instance.Passwords.Get((int)((DataItem)sender).Tag);
                        result = new PasswordWindow(password) { Owner = this }.ShowDialogForResult<string>();
                        break;
                    case NOTES_SECTION:
                        Note? note = DB.Instance.Notes.Get((int)((DataItem)sender).Tag);
                        result = new NoteWindow(note) { Owner = this }.ShowDialogForResult<string>();
                        break;
                    case CARDS_SECTION:
                        Card? card = DB.Instance.Cards.Get((int)((DataItem)sender).Tag);
                        result = new CardWindow(card) { Owner = this }.ShowDialogForResult<string>();
                        break;
                    case DOCUMENTS_SECTION:
                        Document? document = DB.Instance.Documents.Get((int)((DataItem)sender).Tag);
                        result = new DocumentWindow(document) { Owner = this }.ShowDialogForResult<string>();
                        break;
                    default:
                        break;
                }

                //If the item is edited, reloads
                if (result != null) ReloadCurrentSection();
            }
        }

        #endregion
    }
}
