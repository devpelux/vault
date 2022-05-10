using FullControls.Controls;
using FullControls.SystemComponents;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for display and edit a card.
    /// </summary>
    public partial class CardWindow : AvalonWindow, IDialog
    {
        private readonly Card? card;
        private readonly List<Category> categories;

        private object? Result = null;

        /// <summary>
        /// Initializes a new <see cref="NoteWindow"/> with the specified note.
        /// If the note is null, the window will create a new note, otherwise will display and edit the specified note.
        /// </summary>
        public CardWindow(Card? card)
        {
            InitializeComponent();
            this.card = card;
            categories = DB.Instance.Categories.GetAll();
        }

        /// <inheritdoc/>
        public object? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the card details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utility.LoadCategoryItems(CardCategory, (Style)FindResource("DarkComboBoxItemPlus"), categories);

            if (card != null)
            {
                CardRequestKey.IsChecked = card.IsLocked;
                CardLabel.Text = card.Name;
                CardOwner.Text = card.Owner;
                CardType.Text = card.Type;
                CardNumber.Text = card.Number;
                CardSecureCode.Text = card.Cvv;
                CardExpiration.Text = "";
                CardNote.Text = card.Notes;
                CardCategory.SelectedIndex = categories.FindIndex(category => category.Name == card.Category);
                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                CardCategory.SelectedIndex = 0;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        #region Commands

        private void CopyValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is TextBoxPlus textBox)
            {
                e.CanExecute = textBox.TextLength > 0;
            }
            else if (sender is PasswordBoxPlus passwordBox)
            {
                e.CanExecute = passwordBox.PasswordLength > 0;
            }
        }

        private void ReplaceValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText();
        }

        private void CopyValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is TextBoxPlus textBox) textBox.CopyAll();
            else if (sender is PasswordBoxPlus passwordBox) passwordBox.CopyAll();
        }

        private void ReplaceValue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is TextBoxPlus textBox)
            {
                textBox.Clear();
                textBox.Paste();
            }
            else if (sender is PasswordBoxPlus passwordBox)
            {
                passwordBox.Clear();
                passwordBox.Paste();
            }
        }

        #endregion

        /// <summary>
        /// Executed when the cancel button is clicked.
        /// Closes the window.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// Executed when the ok button is clicked.
        /// Edits the card if is not null, otherwise creates a new card.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (card == null) AddCard();
            else EditCard();

            Result = "edit";
            Close();
        }

        /// <summary>
        /// Adds a new card.
        /// </summary>
        private void AddCard()
        {
            string category = categories[CardCategory.SelectedIndex].Name;
            string name = CardLabel.Text;
            string owner = CardOwner.Text;
            string number = CardNumber.Text;
            string type = CardType.Text;
            string cvv = CardSecureCode.Text;
            string? iban = null;
            long expiration = -1; //CardExpiration.Text;
            string? notes = CardNote.Text;
            bool isLocked = CardRequestKey.IsChecked ?? false;

            Card newCard = new(category, name, owner, number, type, cvv, iban, expiration, notes, isLocked);

            DB.Instance.Cards.Add(newCard);
        }

        /// <summary>
        /// Edit the card.
        /// </summary>
        private void EditCard()
        {
            if (card == null) return;

            int id = card.Id;
            string category = categories[CardCategory.SelectedIndex].Name;
            string name = CardLabel.Text;
            string owner = CardOwner.Text;
            string number = CardNumber.Text;
            string type = CardType.Text;
            string cvv = CardSecureCode.Text;
            string? iban = null;
            long expiration = -1; //CardExpiration.Text;
            string? notes = CardNote.Text;
            bool isLocked = CardRequestKey.IsChecked ?? false;

            Card newCard = new(id, category, name, owner, number, type, cvv, iban, expiration, notes, isLocked);

            DB.Instance.Cards.Update(newCard);
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the note if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (card == null) return;

            DB.Instance.Cards.Remove(card.Id);

            Result = "edit";
            Close();
        }
    }
}
