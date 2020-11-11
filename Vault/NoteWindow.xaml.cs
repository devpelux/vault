using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.CustomControls;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle note memorizzate.
    /// </summary>
    public partial class NoteWindow : Window, IDialogWindow
    {
        private readonly Note note;

        private string Result = "";

        public const string NONE = "NoteWindow.NONE";
        public const string EDIT = "NoteWindow.EDIT";


        public NoteWindow(Note note)
        {
            InitializeComponent();
            this.note = note != null ? Notes.Decrypt(note, Global.Instance.Key) : null;
        }

        public string GetResult() => Result;

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (note != null)
            {
                NoteTitle.Text = note.Title;
                NoteDescription.Text = note.Description;
                NoteRequestKey.IsChecked = note.RequestKey;
                NoteText.Text = note.Text;
                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                Delete.Visibility = Visibility.Hidden;
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
            Note note = new Note
                (
                    Passwords.NewID,
                    Global.Instance.UserID,
                    NoteTitle.Text,
                    "Generic",
                    NoteDescription.Text,
                    NoteRequestKey.IsChecked ?? false,
                    NoteText.Text
                );
            VaultDB.Instance.Notes.AddRecord(Notes.Encrypt(note, Global.Instance.Key));
        }

        private void EditElement()
        {
            Note editedNote = note with
            {
                UserID = Global.Instance.UserID,
                Title = NoteTitle.Text,
                Category = "Generic",
                Description = NoteDescription.Text,
                RequestKey = NoteRequestKey.IsChecked ?? false,
                Text = NoteText.Text
            };
            VaultDB.Instance.Notes.UpdateRecord(Notes.Encrypt(editedNote, Global.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Notes.RemoveRecord(note.ID);
            Result = EDIT;
            Close();
        }
    }
}
