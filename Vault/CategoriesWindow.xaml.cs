using FullControls.Controls;
using FullControls.SystemComponents;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using Vault.Properties;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for managing the categories.
    /// </summary>
    public partial class CategoriesWindow : AvalonWindow, IDialog<bool>
    {
        private List<Category> categories = new();
        private Category? selectedCategory = null;

        /// <summary>
        /// EditResult: false = no edits, true = edits. (default: false)
        /// </summary>
        private bool EditResult = false;

        /// <summary>
        /// Initializes a new <see cref="CategoriesWindow"/>.
        /// </summary>
        public CategoriesWindow()
        {
            InitializeComponent();
        }

        /// <inheritdoc/>
        public bool GetResult() => EditResult;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the categories details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Reload();
        }

        /// <summary>
        /// Reloads the categories details.
        /// </summary>
        private void Reload()
        {
            //Gets all the categories, then hides the default "none" category to avoid editing it.
            categories = DB.Instance.Categories.GetAll();
            categories.Remove(Category.None);

            CategoryList.ItemsSource = categories;

            //If there is no editable category (the default "none" is not editable), displays a warning.
            //Otherwise displays the categories.
            if (categories.Count == 0)
            {
                CategoryViewer.Visibility = Visibility.Collapsed;
                NoCategory.Visibility = Visibility.Visible;
            }
            else
            {
                CategoryViewer.Visibility = Visibility.Visible;
                NoCategory.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Executed when a category is checked.
        /// Loads the category details to edit it.
        /// </summary>
        private void Category_Checked(object sender, RoutedEventArgs e)
        {
            string? categoryName = ((Switcher)sender).Content.ToString();

            selectedCategory = categories.Find(c => c.Name == categoryName);

            //Uses string.Empty to avoid null strings.
            CategoryName.Text = selectedCategory?.Name ?? string.Empty;
            CategoryLabel.Text = selectedCategory?.Label ?? string.Empty;
        }

        /// <summary>
        /// Executed when a category is unchecked.
        /// Cleans the category details.
        /// </summary>
        private void Category_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedCategory = null;
            CategoryName.Text = string.Empty;
            CategoryLabel.Text = string.Empty;
        }

        /// <summary>
        /// Executed when a key is pressed.
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                //If is pressed ENTER + CTRL (DX or SX), removes the category details and reloads the window. (cancels the operation)
                selectedCategory = null;
                CategoryName.Text = string.Empty;
                CategoryLabel.Text = string.Empty;

                MoveFocus(new(FocusNavigationDirection.Last));

                Reload();
            }
            else if (e.Key == Key.Enter)
            {
                //If is pressed only ENTER, saves the category, then removes the category details and reloads the window. (saves the operation)
                SaveCategory();

                selectedCategory = null;
                CategoryName.Text = string.Empty;
                CategoryLabel.Text = string.Empty;

                MoveFocus(new(FocusNavigationDirection.Last));

                Reload();
            }
        }

        /// <summary>
        /// Saves the details for the edited category or the new category.
        /// </summary>
        private void SaveCategory()
        {
            Category newCategory = new(CategoryName.Text, CategoryLabel.Text);

            if (selectedCategory == null)
            {
                if (newCategory.Name != string.Empty)
                {
                    if (!DB.Instance.Categories.Add(newCategory))
                    {
                        new MessageWindow(Strings.CategoryDuplicatedAdd, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
                    }
                    else EditResult = true;
                }
            }
            else
            {
                if (newCategory.Name != string.Empty)
                {
                    newCategory.IsExpanded = selectedCategory.IsExpanded;
                    if (!DB.Instance.Categories.Update(selectedCategory.Name, newCategory))
                    {
                        new MessageWindow(Strings.CategoryDuplicatedEdit, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
                    }
                    else EditResult = true;
                }
                else if (new ConfirmWindow(Strings.ConfirmDelete, Strings.Warning, MessageBoxImage.Question).ShowDialogForResult<bool>())
                {
                    if (!DB.Instance.Categories.Remove(selectedCategory.Name))
                    {
                        new MessageWindow(Strings.CategoryUsed, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
                    }
                    else EditResult = true;
                }
            }
        }
    }
}
