using FullControls.SystemComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for display and edit a note.
    /// </summary>
    public partial class NoteWindow : AvalonWindow, IDialog
    {
        private readonly Note? note;
        private readonly List<Category> categories;

        private object? Result = null;

        /// <summary>
        /// Initializes a new <see cref="NoteWindow"/> with the specified note.
        /// If the note is null, the window will create a new note, otherwise will display and edit the specified note.
        /// </summary>
        public NoteWindow(Note? note)
        {
            InitializeComponent();
            this.note = note;
            categories = DB.Instance.Categories.GetAll();
        }

        /// <inheritdoc/>
        public object? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the note details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utility.LoadCategoryItems(NoteCategory, (Style)FindResource("DarkComboBoxItemPlus"), categories);
            if (note != null)
            {
                NoteRequestKey.IsChecked = note.IsLocked;
                NoteTitle.Text = note.Title;
                NoteText.Text = note.Text;
                NoteCategory.SelectedIndex = categories.FindIndex(category => category.Name == note.Category);
                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                NoteCategory.SelectedIndex = 0;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Executed when the cancel button is clicked.
        /// Closes the window.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// Executed when the ok button is clicked.
        /// Edits the note if is not null, otherwise creates a new note.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (note == null) AddNote();
            else EditNote();

            Result = "edit";
            Close();
        }

        /// <summary>
        /// Adds a new note.
        /// </summary>
        private void AddNote()
        {
            string category = categories[NoteCategory.SelectedIndex].Name;
            string title = NoteTitle.Text;
            string text = NoteText.Text;
            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            bool isLocked = NoteRequestKey.IsChecked ?? false;

            Note newNote = new(category, title, text, timestamp, isLocked);

            DB.Instance.Notes.Add(newNote);
        }

        /// <summary>
        /// Edit the note.
        /// </summary>
        private void EditNote()
        {
            if (note == null) return;

            int id = note.Id;
            string category = categories[NoteCategory.SelectedIndex].Name;
            string title = NoteTitle.Text;
            string text = NoteText.Text;
            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            bool isLocked = NoteRequestKey.IsChecked ?? false;

            Note newNote = new(id, category, title, text, timestamp, isLocked);

            DB.Instance.Notes.Update(newNote);
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the note if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (note == null) return;

            DB.Instance.Notes.Remove(note.Id);

            Result = "edit";
            Close();
        }
    }
}
