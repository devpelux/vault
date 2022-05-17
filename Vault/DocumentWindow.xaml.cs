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
    /// Window for display and edit a document.
    /// </summary>
    public partial class DocumentWindow : AvalonWindow, IDialog
    {
        private readonly Document? document;
        private readonly List<Category> categories;

        /// <summary>
        /// Result: "edited", "deleted", null = nothing. (default: null)
        /// </summary>
        private object? Result = null;

        /// <summary>
        /// Initializes a new <see cref="DocumentWindow"/> with the specified document.
        /// If the document is null, the window will create a new document, otherwise will display and edit the specified document.
        /// </summary>
        public DocumentWindow(Document? document)
        {
            InitializeComponent();

            this.document = document;
            categories = DB.Instance.Categories.GetAll();

            //Adds the field commands
            FieldCommands.AddFieldCommands(CommandBindings);
        }

        /// <inheritdoc/>
        public object? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the document details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utility.LoadCategoryItems(DocumentCategory, (Style)FindResource("DarkComboBoxItemPlus"), categories);

            if (document != null)
            {
                DocumentCategory.SelectedIndex = categories.FindIndex(category => category.Name == document.Category);

                DocumentName.Text = document.Name;
                DocumentOwner.Text = document.Owner;
                DocumentCode.Text = document.Code;
                DocumentNotes.Text = document.Notes;

                DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(document.Expiration);
                DocumentExpirationYear.Text = time.Year.ToString();
                DocumentExpirationMonth.Text = time.Month.ToString();
                DocumentExpirationDay.Text = time.Day.ToString();

                Reauthenticate.IsChecked = document.IsLocked;

                Delete.Visibility = Visibility.Visible;
            }
            else
            {
                DocumentCategory.SelectedIndex = 0;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Executed when the save button is clicked.
        /// Edits the document if is not null, otherwise creates a new document.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (document == null) AddDocument();
            else EditDocument();

            Result = "edited";
            Close();
        }

        /// <summary>
        /// Executed when the delete button is clicked.
        /// Deletes the document if is not null.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (document == null) return;

            DB.Instance.Documents.Remove(document.Id);

            Result = "deleted";
            Close();
        }

        /// <summary>
        /// Adds a new document.
        /// </summary>
        private void AddDocument()
        {
            string category = categories[DocumentCategory.SelectedIndex].Name;
            string name = DocumentName.Text;
            string owner = DocumentOwner.Text;
            string code = DocumentCode.Text;

            int year = DocumentExpirationYear.Text.IsInt() ? int.Parse(DocumentExpirationYear.Text) : 0;
            int month = DocumentExpirationMonth.Text.IsInt() ? int.Parse(DocumentExpirationMonth.Text) : 0;
            int day = DocumentExpirationDay.Text.IsInt() ? int.Parse(DocumentExpirationDay.Text) : 0;
            DateTimeOffset time = new DateTime(year, month, day);

            long expiration = time.ToUnixTimeSeconds();

            string? notes = DocumentNotes.Text;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Document newDocument = new(category, name, owner, code, expiration, notes, isLocked);

            DB.Instance.Documents.Add(newDocument);
        }

        /// <summary>
        /// Edits the document.
        /// </summary>
        private void EditDocument()
        {
            int id = document?.Id ?? -1;

            string category = categories[DocumentCategory.SelectedIndex].Name;
            string name = DocumentName.Text;
            string owner = DocumentOwner.Text;
            string code = DocumentCode.Text;

            int year = DocumentExpirationYear.Text.IsInt() ? int.Parse(DocumentExpirationYear.Text) : 0;
            int month = DocumentExpirationMonth.Text.IsInt() ? int.Parse(DocumentExpirationMonth.Text) : 0;
            int day = DocumentExpirationDay.Text.IsInt() ? int.Parse(DocumentExpirationDay.Text) : 0;
            DateTimeOffset time = new DateTime(year, month, day);

            long expiration = time.ToUnixTimeSeconds();

            string? notes = DocumentNotes.Text;
            bool isLocked = Reauthenticate.IsChecked ?? false;

            Document newDocument = new(id, category, name, owner, code, expiration, notes, isLocked);

            DB.Instance.Documents.Update(newDocument);
        }
    }
}