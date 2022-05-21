using FullControls.SystemComponents;
using System.Collections.Generic;
using System.Windows;
using Vault.Core.Database;

namespace Vault
{
    /// <summary>
    /// Window for display and edit the weak passwords list.
    /// </summary>
    public partial class WeakPasswordsWindow : AvalonWindow
    {
        /// <summary>
        /// Initializes a new <see cref="WeakPasswordsWindow"/>.
        /// </summary>
        public WeakPasswordsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the weak passwords list.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            List<string> weakPasswords = DB.Instance.WeakPasswords.GetAll();

            foreach (string password in weakPasswords)
            {
                WeakPasswordsList.Text += password + "\n";
            }
        }

        /// <summary>
        /// Executed when the save button is clicked.
        /// Updates the weak passwords list.
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DB.Instance.WeakPasswords.Clear();

            for (int i = 0; i < WeakPasswordsList.LineCount; i++)
            {
                string value = WeakPasswordsList.GetLineText(i).Trim();
                if (value.Length > 0) DB.Instance.WeakPasswords.Add(value);
            }

            Close();
        }
    }
}
