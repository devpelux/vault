using CoreTools.Extensions;
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
    /// Window for display and edit a card.
    /// </summary>
    public partial class CardWindow : AvalonWindow, IDialog<string>
    {
        private readonly Card? card;
        private readonly List<Category> categories;

        /// <summary>
        /// Result: "edited", "deleted", null = nothing. (default: null)
        /// </summary>
        private string? Result = null;

        /// <summary>
        /// Initializes a new <see cref="CardWindow"/> with the specified card.
        /// If the card is null, the window will create a new card, otherwise will display and edit the specified card.
        /// </summary>
        public CardWindow(Card? card)
        {
            InitializeComponent();

            this.card = card;
            categories = DB.Instance.Categories.GetAll();

            //Adds the field commands
            FieldCommands.AddFieldCommands(CommandBindings);
        }

        /// <inheritdoc/>
        public string? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the card details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Utility.LoadCategoryItems(CardCategory, (Style)FindResource("DarkComboBoxItemPlus"), categories);

            if (card != null)
            {
                CardCategory.SelectedIndex = categories.FindIndex(category => category.Name == card.Category);

                CardName.Text = card.Name;
                CardOwner.Text = card.Owner;
                CardNumber.Text = card.Number;
                CardType.Text = card.Type;
                CardCvv.Text = card.Cvv;
                CardIban.Text = card.Iban;
                CardNotes.Text = card.Notes;

                DateTime utcTime = DateTimeOffset.FromUnixTimeSeconds(card.Expiration).UtcDateTime;
                CardExpirationYear.Text = utcTime.Year.ToString();
                CardExpirationMonth.Text = utcTime.Month.ToString();

                Reauthenticate.IsChecked = card.IsLocked;

                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                CardCategory.SelectedIndex = 0;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Executed when the save button is clicked.
        /// Edits the card if is not null, otherwise creates a new card.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (card == null) AddCard();
                else EditCard();

                Result = "edited";
                Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                new MessageWindow(Strings.InvalidDate, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
            }
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the card if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (card == null) return;

            if (new ConfirmWindow(Strings.ConfirmDelete, Strings.Warning, MessageBoxImage.Question).ShowDialogForResult<bool>())
            {
                DB.Instance.Cards.Remove(card.Id);

                Result = "deleted";
                Close();
            }
        }

        /// <summary>
        /// Adds a new card.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private void AddCard()
        {
            string category = categories[CardCategory.SelectedIndex].Name;
            string name = CardName.Text;
            string owner = CardOwner.Text;
            string number = CardNumber.Text;
            string type = CardType.Text;
            string cvv = CardCvv.Text;
            string? iban = CardIban.Text;

            int year = CardExpirationYear.Text.ToInt(-1);
            int month = CardExpirationMonth.Text.ToInt(-1);

            long expiration;

            if (year != -1 || month != -1)
            {
                DateTimeOffset utcTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), 1, 0, 0, 0, DateTimeKind.Utc);
                expiration = utcTime.ToUnixTimeSeconds();
            }
            else
            {
                throw new ArgumentException("Invalid date.");
            }

            string? notes = CardNotes.Text;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Card newCard = new(category, name, owner, number, type, cvv, iban, expiration, notes, isLocked);

            DB.Instance.Cards.Add(newCard);
        }

        /// <summary>
        /// Edits the card.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private void EditCard()
        {
            int id = card?.Id ?? -1;

            string category = categories[CardCategory.SelectedIndex].Name;
            string name = CardName.Text;
            string owner = CardOwner.Text;
            string number = CardNumber.Text;
            string type = CardType.Text;
            string cvv = CardCvv.Text;
            string? iban = CardIban.Text;

            int year = CardExpirationYear.Text.ToInt(-1);
            int month = CardExpirationMonth.Text.ToInt(-1);

            long expiration;

            if (year != -1 || month != -1)
            {
                DateTimeOffset utcTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), 1, 0, 0, 0, DateTimeKind.Utc);
                expiration = utcTime.ToUnixTimeSeconds();
            }
            else
            {
                throw new ArgumentException("Invalid date.");
            }

            string? notes = CardNotes.Text;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Card newCard = new(id, category, name, owner, number, type, cvv, iban, expiration, notes, isLocked);

            DB.Instance.Cards.Update(newCard);
        }
    }
}
