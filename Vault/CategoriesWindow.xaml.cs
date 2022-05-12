using FullControls.Controls;
using FullControls.SystemComponents;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Vault.Core.Database;
using Vault.Core.Database.Data;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for managing the categories.
    /// </summary>
    public partial class CategoriesWindow : AvalonWindow, IDialog
    {
        private List<Category> categories = new();

        private Category? selectedCategory = null;

        /// <summary>
        /// Initializes a new <see cref="CategoriesWindow"/>.
        /// </summary>
        public CategoriesWindow()
        {
            InitializeComponent();
        }

        /// <inheritdoc/>
        public object? GetResult() => null;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the categories details.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e) => Reload();

        /// <summary>
        /// Reloads the categories details.
        /// </summary>
        private void Reload()
        {
            //Gets all the categories, then hides the default "none" category to avoid editing it.
            categories = DB.Instance.Categories.GetAll();
            categories.Remove(Category.None);

            CategoryList.ItemsSource = categories;

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

        private void Category_Checked(object sender, RoutedEventArgs e)
        {
            string? categoryName = ((Switcher)sender).Content.ToString();

            selectedCategory = categories.Find(c => c.Name == categoryName);

            CategoryName.Text = selectedCategory?.Name ?? string.Empty;
            CategoryLabel.Text = selectedCategory?.Label ?? string.Empty;
        }

        private void Category_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedCategory = null;
            CategoryName.Text = string.Empty;
            CategoryLabel.Text = string.Empty;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Category newCategory = new(CategoryName.Text, CategoryLabel.Text);

                if (selectedCategory == null)
                {
                    if (newCategory.Name != string.Empty) DB.Instance.Categories.Add(newCategory);
                }
                else
                {
                    if (newCategory.Name != string.Empty) DB.Instance.Categories.Update(selectedCategory.Name, newCategory);
                    else DB.Instance.Categories.Remove(selectedCategory.Name);
                }

                selectedCategory = null;
                CategoryName.Text = string.Empty;
                CategoryLabel.Text = string.Empty;

                Reload();
            }
        }
    }
}
