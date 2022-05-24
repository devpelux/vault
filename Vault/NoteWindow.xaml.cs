using FullControls.SystemComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using Vault.Core;
using Vault.Core.Controls;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using Vault.Properties;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for display and edit a note.
    /// </summary>
    public partial class NoteWindow : AvalonWindow, IDialog<string>
    {
        private readonly Note? note;
        private readonly List<Category> categories;
        private readonly DateTimeOffset utcNow = DateTimeOffset.UtcNow;

        /// <summary>
        /// Result: "edited", "deleted", null = nothing. (default: null)
        /// </summary>
        private string? Result = null;

        /// <summary>
        /// Initializes a new <see cref="NoteWindow"/> with the specified note.
        /// If the note is null, the window will create a new note, otherwise will display and edit the specified note.
        /// </summary>
        public NoteWindow(Note? note)
        {
            InitializeComponent();

            this.note = note;
            categories = DB.Instance.Categories.GetAll();

            //Adds the field commands
            FieldCommands.AddFieldCommands(CommandBindings);
        }

        /// <inheritdoc/>
        public string? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the note details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Utility.LoadCategoryItems(NoteCategory, (Style)FindResource("DarkComboBoxItemPlus"), categories);

            if (note != null)
            {
                NoteCategory.SelectedIndex = categories.FindIndex(category => category.Name == note.Category);

                NoteTitle.Text = note.Title;
                NoteText.Text = note.Text;

                NoteTimestamp.Text = Utility.FormatDate(DateTimeOffset.FromUnixTimeSeconds(note.Timestamp));

                Reauthenticate.IsChecked = note.IsLocked;

                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                NoteCategory.SelectedIndex = 0;

                NoteTimestamp.Text = Utility.FormatDate(utcNow);

                Delete.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Executed when the save button is clicked.
        /// Edits the note if is not null, otherwise creates a new note.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (note == null) AddNote();
            else EditNote();

            Result = "edited";
            Close();
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the note if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (note == null) return;

            if (new ConfirmWindow(Strings.ConfirmDelete, Strings.Warning, MessageBoxImage.Question).ShowDialogForResult<bool>())
            {
                DB.Instance.Notes.Remove(note.Id);

                Result = "deleted";
                Close();
            }
        }

        /// <summary>
        /// Adds a new note.
        /// </summary>
        private void AddNote()
        {
            string category = categories[NoteCategory.SelectedIndex].Name;
            string title = NoteTitle.Text;
            string text = NoteText.Text;
            long timestamp = utcNow.ToUnixTimeSeconds();
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Note newNote = new(category, title, text, timestamp, isLocked);

            DB.Instance.Notes.Add(newNote);
        }

        /// <summary>
        /// Edits the note.
        /// </summary>
        private void EditNote()
        {
            if (note == null) return;

            int id = note.Id;

            string category = categories[NoteCategory.SelectedIndex].Name;
            string title = NoteTitle.Text;
            string text = NoteText.Text;
            long timestamp = note.Timestamp;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Note newNote = new(id, category, title, text, timestamp, isLocked);

            DB.Instance.Notes.Update(newNote);
        }
    }
}
