using FullControls.SystemComponents;
using System;
using System.Windows;
using Vault.Core.Settings;

namespace Vault
{
    /// <summary>
    /// Window for changing the settings.
    /// </summary>
    public partial class SettingsWindow : AvalonWindow
    {
        private bool loaded;

        /// <summary>
        /// Initializes a new <see cref="SettingsWindow"/>.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads all the editable settings values in the window.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemSettings.StartOnStartup.HasValue) AutoStart.IsChecked = SystemSettings.StartOnStartup.Value;
            else AutoStart.IsEnabled = false;

            StartHided.IsChecked = Settings.Instance.GetSetting("start_hided", false);
            HideOnClose.IsChecked = Settings.Instance.GetSetting("exit_explicit", true);

            loaded = true;
        }

        /// <summary>
        /// Executed when the window is closed.
        /// Saves the settings.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            Settings.Instance.Save();
        }

        #region Checkboxes for to change the settings

        private void AutoStart_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) SystemSettings.StartOnStartup = true;
        }

        private void AutoStart_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) SystemSettings.StartOnStartup = false;
        }

        private void StartHided_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.SetSetting("start_hided", true);
        }

        private void StartHided_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.SetSetting("start_hided", false);
        }

        private void HideOnClose_Checked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.SetSetting("exit_explicit", true);
        }

        private void HideOnClose_Unchecked(object sender, RoutedEventArgs e)
        {
            if (loaded) Settings.Instance.SetSetting("exit_explicit", false);
        }

        #endregion
    }
}
