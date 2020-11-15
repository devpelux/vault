using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.CustomControls;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle carte memorizzate.
    /// </summary>
    public partial class CardWindow : Window, IDialogWindow
    {
        private readonly Card card;

        private string Result = "";

        public const string NONE = "CardWindow.NONE";
        public const string EDIT = "CardWindow.EDIT";


        public CardWindow(Card card)
        {
            InitializeComponent();
            this.card = card != null ? Cards.Decrypt(card, Session.Instance.Key) : null;
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
                    1,
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
                Category = 1,
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
