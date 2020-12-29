using FullControls;
using System;
using System.Windows;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra delle impostazioni.
    /// </summary>
    public partial class SettingsWindow : EWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SettingsWrapper.SaveAll();
        }
    }
}
