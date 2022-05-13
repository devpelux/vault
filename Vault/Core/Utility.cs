using FullControls.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Media.Imaging;
using Vault.Core.Database.Data;

namespace Vault.Core
{
    public static class Utility
    {
        public static void LoadCategoryItems(ComboBoxPlus comboBox, Style style, List<Category> categories)
        {
            comboBox.Items.Clear();
            foreach (Category category in categories)
            {
                ComboBoxItemPlus comboBoxItem = new()
                {
                    Style = style,
                    Content = category.Name
                };
                comboBox.Items.Add(comboBoxItem);
            }
        }

        public static bool ComparePasswords(SecureString password1, SecureString password2)
        {
            byte[] salt = Encryptor.GenerateSalt();
            string hashPassword1 = Encryptor.ConvertToString(Encryptor.GenerateKey(password1, salt, 1000));
            string hashPassword2 = Encryptor.ConvertToString(Encryptor.GenerateKey(password2, salt, 1000));
            return hashPassword1.Equals(hashPassword2);
        }

        public static T ConvertFromString<T>(string str)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(str);
        }

        public static BitmapImage LoadIconFromUri(string uri)
        {
            BitmapImage icon = new();
            icon.BeginInit();
            icon.UriSource = new(uri);
            icon.EndInit();
            return icon;
        }
    }
}
