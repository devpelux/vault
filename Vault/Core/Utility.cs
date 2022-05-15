using FullControls.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Windows;
using Vault.Core.Database.Data;

namespace Vault.Core
{
    /// <summary>
    /// Provides some utility methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Loads the category items in the specified combobox with the specified style for the items.
        /// </summary>
        public static void LoadCategoryItems(ComboBoxPlus comboBox, Style style, List<Category> categories)
        {
            comboBox.Items.Clear();
            foreach (Category category in categories)
            {
                ComboBoxItemPlus comboBoxItem = new()
                {
                    Style = style,
                    Content = category != Category.None ? category.Name : "Non categorizzato",
                    Tag = category.Name
                };
                comboBox.Items.Add(comboBoxItem);
            }
        }

        /// <summary>
        /// Compares two secure strings for equality.
        /// </summary>
        public static bool ComparePasswords(SecureString password1, SecureString password2)
        {
            byte[] salt = Encryptor.GenerateSalt();
            string hashPassword1 = Encryptor.ConvertToString(Encryptor.GenerateKey(password1, salt, 1000));
            string hashPassword2 = Encryptor.ConvertToString(Encryptor.GenerateKey(password2, salt, 1000));
            return hashPassword1.Equals(hashPassword2);
        }

        /// <summary>
        /// Converts a string to the specific value using a <see cref="TypeDescriptor"/> converter.
        /// </summary>
        public static T? ConvertFromString<T>(string str)
        {
            return (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(str);
        }
    }
}
