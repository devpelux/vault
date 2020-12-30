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
        private bool loaded;


        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoStart.IsChecked = SettingsWrapper.StartOnStartup;
            StartHided.IsChecked = SettingsWrapper.StartHided;
            RememberDBPassword.IsChecked = RememberDBPassword.IsEnabled = SettingsWrapper.DBSavedPassword != "";
            loaded = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SettingsWrapper.SaveAll();
        }

        private void AutoStart_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) SettingsWrapper.StartOnStartup = true;
        }

        private void AutoStart_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) SettingsWrapper.StartOnStartup = false;
        }
        
        private void StartHided_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) SettingsWrapper.StartHided = true;
        }

        private void StartHided_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) SettingsWrapper.StartHided = false;
        }

        private void RememberDBPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                SettingsWrapper.DBSavedPassword = "";
                RememberDBPassword.IsEnabled = false;
            }
        }
    }
}
