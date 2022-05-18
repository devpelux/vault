﻿using CoreTools.Extensions;
using FullControls.SystemComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using Vault.Core;
using Vault.Core.Controls;
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

        /// <summary>
        /// Result: "edited", "deleted", null = nothing. (default: null)
        /// </summary>
        private object? Result = null;

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
                new MessageWindow("Data non valida!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
            }
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the card if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (card == null) return;

            DB.Instance.Cards.Remove(card.Id);

            Result = "deleted";
            Close();
        }

        /// <summary>
        /// Adds a new card.
        /// </summary>
        private void AddCard()
        {
            string category = categories[CardCategory.SelectedIndex].Name;
            string name = CardName.Text;
            string owner = CardOwner.Text;
            string number = CardNumber.Text;
            string type = CardType.Text;
            string cvv = CardCvv.Text;
            string? iban = CardIban.Text;

            int year = CardExpirationYear.Text.IsInt() ? int.Parse(CardExpirationYear.Text) : 1;
            int month = CardExpirationMonth.Text.IsInt() ? int.Parse(CardExpirationMonth.Text) : 1;
            DateTimeOffset utcTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), 1, 0, 0, 0, DateTimeKind.Utc);

            long expiration = utcTime.ToUnixTimeSeconds();

            string? notes = CardNotes.Text;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Card newCard = new(category, name, owner, number, type, cvv, iban, expiration, notes, isLocked);

            DB.Instance.Cards.Add(newCard);
        }

        /// <summary>
        /// Edits the card.
        /// </summary>
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

            int year = CardExpirationYear.Text.IsInt() ? int.Parse(CardExpirationYear.Text) : 1;
            int month = CardExpirationMonth.Text.IsInt() ? int.Parse(CardExpirationMonth.Text) : 1;
            DateTimeOffset utcTime = new DateTime(Math.Clamp(year, 1, 9999), Math.Clamp(month, 1, 12), 1, 0, 0, 0, DateTimeKind.Utc);

            long expiration = utcTime.ToUnixTimeSeconds();

            string? notes = CardNotes.Text;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Card newCard = new(id, category, name, owner, number, type, cvv, iban, expiration, notes, isLocked);

            DB.Instance.Cards.Update(newCard);
        }
    }
}
