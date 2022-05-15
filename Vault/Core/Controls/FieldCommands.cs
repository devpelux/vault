using FullControls.Controls;
using System.Windows;
using System.Windows.Input;

namespace Vault.Core.Controls
{
    /// <summary>
    /// Handles the copy and replace commands for the fields.
    /// </summary>
    public static class FieldCommands
    {
        /// <summary>
        /// Command to copy a value from a field.
        /// </summary>
        public static RoutedCommand CopyValue { get; } = new(nameof(CopyValue), typeof(FieldCommands));

        /// <summary>
        /// Command to replace a value of a field.
        /// </summary>
        public static RoutedCommand ReplaceValue { get; } = new(nameof(ReplaceValue), typeof(FieldCommands));

        /// <summary>
        /// Adds the field commands to the specified <see cref="CommandBindingCollection"/>.
        /// </summary>
        public static void AddFieldCommands(CommandBindingCollection commandBindings)
        {
            commandBindings.Add(new CommandBinding(CopyValue, OnCopyValueExecuted, OnCopyValueCanExecute));
            commandBindings.Add(new CommandBinding(ReplaceValue, OnReplaceValueExecuted, OnReplaceValueCanExecute));
        }

        /// <summary>
        /// Enables copying value if the field has text.
        /// </summary>
        public static void OnCopyValueCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Source is TextBoxPlus textBox)
            {
                e.CanExecute = textBox.TextLength > 0;
            }
            else if (e.Source is PasswordBoxPlus passwordBox)
            {
                e.CanExecute = passwordBox.PasswordLength > 0;
            }
            else e.CanExecute = false;
        }

        /// <summary>
        /// Enables replacing value if the clipboard has text.
        /// </summary>
        public static void OnReplaceValueCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText();
        }

        /// <summary>
        /// Copies the value from a field.
        /// </summary>
        public static void OnCopyValueExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Source is TextBoxPlus textBox) textBox.CopyAll();
            else if (e.Source is PasswordBoxPlus passwordBox) passwordBox.CopyAll();
        }

        /// <summary>
        /// Replaces the value of a field.
        /// </summary>
        public static void OnReplaceValueExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Source is TextBoxPlus textBox)
            {
                textBox.Clear();
                textBox.Paste();
            }
            else if (e.Source is PasswordBoxPlus passwordBox)
            {
                passwordBox.Clear();
                passwordBox.Paste();
            }
        }
    }
}
