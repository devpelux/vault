using System.Collections.Generic;
using System.Windows;
using FullControls;

namespace Vault.Core
{
    public static class Utility
    {
        public static void LoadCategoryItems(FullComboBox comboBox, Style style, List<Category> categories)
        {
            comboBox.Items.Clear();
            foreach (Category category in categories)
            {
                FullComboBoxItem comboBoxItem = new FullComboBoxItem
                {
                    Style = style,
                    Content = category.Label
                };
                comboBox.Items.Add(comboBoxItem);
            }
        }
    }
}
