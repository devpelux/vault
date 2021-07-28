using FullControls.Controls;
using FullControls.SystemComponents;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle carte memorizzate.
    /// </summary>
    public partial class CardWindow : FlexWindow, IDialog
    {
        private readonly Card card;
        private readonly List<Category> categories;

        private object Result = null;

        public const string NONE = "CardWindow.NONE";
        public const string EDIT = "CardWindow.EDIT";


        public CardWindow(Card card, List<Category> categories)
        {
            InitializeComponent();
            this.card = card?.Decrypt(Session.Instance.Key);
            this.categories = categories;
        }

        public object GetResult() => Result;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Result == null) Result = NONE;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utility.LoadCategoryItems(CardCategory, (Style)FindResource("ComboBoxItemPlusDark"), categories);
            if (card != null)
            {
                CardRequestKey.IsChecked = card.RequestKey;
                CardLabel.Text = card.Label;
                CardDescription.Text = card.Description;
                CardOwner.Text = card.Owner;
                CardType.Text = card.Type;
                CardNumber.Text = card.Number;
                CardSecureCode.Text = card.SecureCode;
                CardExpiration.Text = card.Expiration;
                CardNote.Text = card.Note;
                CardCategory.SelectedIndex = categories.FindIndex(c => c.ID == card.Category);
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (card == null) AddElement();
            else EditElement();
            Result = EDIT;
            Close();
        }

        private void AddElement()
        {
            Card card = new(
                    Cards.NewID,
                    Session.Instance.UserID,
                    categories[CardCategory.SelectedIndex].ID,
                    CardRequestKey.IsChecked ?? false,
                    CardLabel.Text,
                    CardDescription.Text,
                    CardOwner.Text,
                    CardType.Text,
                    CardNumber.Text,
                    CardSecureCode.Text,
                    CardExpiration.Text,
                    CardNote.Text
                );
            VaultDB.Instance.Cards.AddRecord(card.Encrypt(Session.Instance.Key));
        }

        private void EditElement()
        {
            Card editedCard = card with
            {
                Category = categories[CardCategory.SelectedIndex].ID,
                RequestKey = CardRequestKey.IsChecked ?? false,
                Label = CardLabel.Text,
                Description = CardDescription.Text,
                Owner = CardOwner.Text,
                Type = CardType.Text,
                Number = CardNumber.Text,
                SecureCode = CardSecureCode.Text,
                Expiration = CardExpiration.Text,
                Note = CardNote.Text
            };
            VaultDB.Instance.Cards.UpdateRecord(editedCard.Encrypt(Session.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Cards.RemoveRecord(card.ID);
            Result = EDIT;
            Close();
        }
    }
}
