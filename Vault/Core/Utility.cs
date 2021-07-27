using System.Collections.Generic;
using System.Security;
using System.Windows;
using FullControls;

namespace Vault.Core
{
    public static class Utility
    {
        public static void LoadCategoryItems(EComboBox comboBox, Style style, List<Category> categories)
        {
            comboBox.Items.Clear();
            foreach (Category category in categories)
            {
                EComboBoxItem comboBoxItem = new()
                {
                    Style = style,
                    Content = category.Label
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
    }
}
