using FullControls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle carte memorizzate.
    /// </summary>
    public partial class CardWindow : EWindow, IDialog
    {
        private readonly Card card;
        private readonly List<Category> categories;

        private string Result = "";

        public const string NONE = "CardWindow.NONE";
        public const string EDIT = "CardWindow.EDIT";


        public CardWindow(Card card, List<Category> categories)
        {
            InitializeComponent();
            this.card = card != null ? Cards.Decrypt(card, Session.Instance.Key) : null;
            this.categories = categories;
        }

        public object GetResult() => Result;

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
            Utility.LoadCategoryItems(CardCategory, (Style)FindResource("EComboBoxItemDark"), categories);
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
            if (card == null) AddElement();
            else EditElement();
            Result = EDIT;
            Close();
        }

        private void AddElement()
        {
            Card card = new Card
                (
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
            VaultDB.Instance.Cards.AddRecord(Cards.Encrypt(card, Session.Instance.Key));
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
            VaultDB.Instance.Cards.UpdateRecord(Cards.Encrypt(editedCard, Session.Instance.Key));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            VaultDB.Instance.Cards.RemoveRecord(card.ID);
            Result = EDIT;
            Close();
        }
    }
}
