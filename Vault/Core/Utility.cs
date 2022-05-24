using FullControls.Controls;
using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Vault.Core.Database.Data;
using Vault.Properties;
using WpfCoreTools.Extensions;

namespace Vault.Core
{
    /// <summary>
    /// Provides some utility methods.
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Gets a value indicating the number of seconds in an unix day.
        /// </summary>
        internal const ulong UNIX_DAY_SECONDS = 86400;

        /// <summary>
        /// Formats the specified date into a string after converting to local time.
        /// </summary>
        internal static string FormatDate(DateTimeOffset date) => date.LocalDateTime.ToString();

        /// <summary>
        /// Format the header for displaying in the preview list.
        /// If the header is null, or empty, returns "...".
        /// If the header is too long, returns a substring with "..." in the end.
        /// </summary>
        internal static string FormatHeader(string? header, int maxLength)
        {
            if (header == null || header.Length == 0) return "...";
            else if (header.Length <= maxLength) return header;
            else return $"{header[..maxLength]}...";
        }

        /// <summary>
        /// Format the category label for displaying.
        /// If the label is empty, returns the name.
        /// </summary>
        internal static string FormatCategoryLabel(Category category)
        {
            if (category == Category.None) return Strings.Uncategorized;
            else if (category.Label.Length == 0) return category.Name;
            else return category.Label;
        }

        /// <summary>
        /// Compares two secure strings for equality.
        /// </summary>
        internal static bool ComparePasswords(SecureString password1, SecureString password2)
        {
            byte[] salt = Encryptor.GenerateSalt();
            string hashPassword1 = Encryptor.ConvertToString(Encryptor.GenerateKey(password1, salt));
            string hashPassword2 = Encryptor.ConvertToString(Encryptor.GenerateKey(password2, salt));
            return hashPassword1.Equals(hashPassword2);
        }

        /// <summary>
        /// Returns the max value from 4 int.
        /// </summary>
        internal static int Max(int a, int b, int c, int d)
        {
            int m = a;
            if (m < b) m = b;
            if (m < c) m = c;
            if (m < d) m = d;
            return m;
        }

        /// <summary>
        /// Animate a <see cref="Brush"/> of an <see cref="UIElement"/> with a specified time.
        /// </summary>
        /// <remarks>
        /// Note: If the initial <see cref="Brush"/> (from) or the final <see cref="Brush"/> (to) are <see langword="null"/>, no animation will be executed.
        /// </remarks>
        internal static void AnimateBrush(UIElement uiElement, DependencyProperty brushProperty, Brush? to, TimeSpan animationTime)
        {
            if (animationTime > TimeSpan.Zero && !uiElement.IsNull(brushProperty) && to != null)
            {
                uiElement.SetValue(brushProperty, ((Brush)uiElement.GetValue(brushProperty)).CloneCurrentValue()); //Unfreeze the brush
                Brush from = (Brush)uiElement.GetValue(brushProperty);

                if (from is SolidColorBrush sbFrom && to is SolidColorBrush sbTo)
                {
                    ColorAnimation animation = new()
                    {
                        From = sbFrom.Color,
                        To = sbTo.Color,
                        Duration = new Duration(animationTime)
                    };
                    from.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    uiElement.SetValue(brushProperty, to);
                }
            }
            else
            {
                uiElement.SetValue(brushProperty, to);
            }
        }

        /// <summary>
        /// Loads the category items in the specified combobox with the specified style for the items.
        /// </summary>
        internal static void LoadCategoryItems(ComboBoxPlus comboBox, Style style, List<Category> categories)
        {
            comboBox.Items.Clear();
            foreach (Category category in categories)
            {
                ComboBoxItemPlus comboBoxItem = new()
                {
                    Style = style,
                    Content = FormatCategoryLabel(category),
                    Tag = category.Name
                };
                comboBox.Items.Add(comboBoxItem);
            }
        }
    }
}
