using FullControls.SystemComponents;
using System;
using System.Windows;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Finestra delle impostazioni.
    /// </summary>
    public partial class SettingsWindow : FlexWindow
    {
        private bool loaded;


        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Instance.StartOnStartup.HasValue) AutoStart.IsChecked = Settings.Instance.StartOnStartup.Value;
            else AutoStart.IsEnabled = false;
            StartHided.IsChecked = Settings.Instance.StartHided;
            HideOnClose.IsChecked = Settings.Instance.HideOnClose;
            RememberDBPassword.IsChecked = RememberDBPassword.IsEnabled = Settings.Instance.DBPassword != null;
            loaded = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Settings.Instance.Save();
        }

        private void AutoStart_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.StartOnStartup = true;
        }

        private void AutoStart_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.StartOnStartup = false;
        }

        private void StartHided_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.StartHided = true;
        }

        private void StartHided_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.StartHided = false;
        }

        private void HideOnClose_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.HideOnClose = true;
        }

        private void HideOnClose_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.HideOnClose = false;
        }

        private void RememberDBPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                Settings.Instance.DBPassword = null;
                RememberDBPassword.IsEnabled = false;
            }
        }
    }
}
