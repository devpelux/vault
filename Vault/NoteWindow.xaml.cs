using FullControls.SystemComponents;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Vault.Core;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle note memorizzate.
    /// </summary>
    public partial class NoteWindow : FlexWindow, IDialog
    {
        private readonly Note note;
        private readonly List<Category> categories;

        private object Result = null;

        public const string NONE = "NoteWindow.NONE";
        public const string EDIT = "NoteWindow.EDIT";


        public NoteWindow(Note note, List<Category> categories)
        {
            InitializeComponent();
            this.note = note?.Decrypt(Session.Instance.Key);
            this.categories = categories;
        }

        public object GetResult() => Result;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Result == null) Result = NONE;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utility.LoadCategoryItems(NoteCategory, (Style)FindResource("ComboBoxItemPlusDark"), categories);
            if (note != null)
            {
                NoteRequestKey.IsChecked = note.RequestKey;
                NoteTitle.Text = note.Title;
                NoteSubtitle.Text = note.Subtitle;
                NoteText.Text = note.Text;
                NoteCategory.SelectedIndex = categories.FindIndex(c => c.ID == note.Category);
                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                NoteCategory.SelectedIndex = 0;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (note == null) AddElement();
            else EditElement();
            Result = EDIT;
            Close();
        }

        private void AddElement()
        {
            Note note = new(
                    Passwords.NewID,
                    Session.Instance.UserID,
                    categories[NoteCategory.SelectedIndex].ID,
                    NoteRequestKey.IsChecked ?? false,
                    NoteTitle.Text,
                    NoteSubtitle.Text,
                    NoteText.Text
                );
            VaultDB.Instance.Notes.AddRecord(note.Encrypt(Session.Instance.Key));
        }

        private void EditElement()
        {
            Note editedNote = note with
            {
                Category = categories[NoteCategory.SelectedIndex].ID,
                RequestKey = NoteRequestKey.IsChecked ?? false,
                Title = NoteTitle.Text,
                Subtitle = NoteSubtitle.Text,
                Text = NoteText.Text
            };
            VaultDB.Instance.Notes.UpdateRecord(editedNote.Encrypt(Session.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Notes.RemoveRecord(note.ID);
            Result = EDIT;
            Close();
        }
    }
}
