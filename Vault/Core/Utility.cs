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
        /// Gets a value indicating the number of seconds in an unix day.
        /// </summary>
        public const ulong UnixDaySeconds = 86400;

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
            string hashPassword1 = Encryptor.ConvertToString(Encryptor.GenerateKey(password1, salt));
            string hashPassword2 = Encryptor.ConvertToString(Encryptor.GenerateKey(password2, salt));
            return hashPassword1.Equals(hashPassword2);
        }

        /// <summary>
        /// Converts a string to the specific value using a <see cref="TypeDescriptor"/> converter.
        /// </summary>
        public static T? ConvertFromString<T>(string str) => (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(str);

        /// <summary>
        /// Returns the max value from 4 int.
        /// </summary>
        public static int Max(int a, int b, int c, int d)
        {
            int m = a;
            if (m < b) m = b;
            if (m < c) m = c;
            if (m < d) m = d;
            return m;
        }
    }
}
