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
        private bool lockCheckboxes;

        /// <summary>
        /// Initializes a new <see cref="SettingsWindow"/>.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            VersionCode.Text = App.GetVersionCode();
        }

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads all the editable settings values in the window.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            LoadSettings();
        }

        /// <summary>
        /// Loads all the editable settings values in the window.
        /// </summary>
        private void LoadSettings()
        {
            lockCheckboxes = true;

            StartOnStartup.IsChecked = SystemSettings.StartOnStartup == true;
            StartHided.IsChecked = Settings.Instance.GetSetting("start_hided", false);
            ExitExplicit.IsChecked = Settings.Instance.GetSetting("exit_explicit", true);

            lockCheckboxes = false;
        }

        /// <summary>
        /// Executed when the window is closed.
        /// Saves the settings.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e) => Settings.Instance.Save();

        #region Checkboxes for to change the settings

        //When a checkbox changes its state, the relative setting is changed.

        private void StartOnStartup_Checked(object sender, RoutedEventArgs e)
        {
            if (lockCheckboxes) return;

            bool? startOnStartup = SystemSettings.StartOnStartup;

            //Checks the actual value of StartOnStartup.
            //If it has not a value then requests if to overwrite it.
            if (startOnStartup.HasValue)
            {
                SystemSettings.StartOnStartup = true;
            }
            else if (new ConfirmWindow("L'avvio automatico è già in uso da un altra istanza! Sovrascrivere?",
                "Attenzione!", MessageBoxImage.Question) { Owner = this }.ShowDialogForResult<bool>())
            {
                SystemSettings.StartOnStartup = true;
            }
            else
            {
                lockCheckboxes = true;
                StartOnStartup.IsChecked = false;
                lockCheckboxes = false;
            }
        }

        private void StartOnStartup_Unchecked(object sender, RoutedEventArgs e)
        {
            if (lockCheckboxes) return;
            SystemSettings.StartOnStartup = false;
        }

        private void StartHided_Checked(object sender, RoutedEventArgs e)
        {
            if (lockCheckboxes) return;
            Settings.Instance.SetSetting("start_hided", true);
            ExitExplicit.IsChecked = true;
        }

        private void StartHided_Unchecked(object sender, RoutedEventArgs e)
        {
            if (lockCheckboxes) return;
            Settings.Instance.SetSetting("start_hided", false);
        }

        private void ExitExplicit_Checked(object sender, RoutedEventArgs e)
        {
            if (lockCheckboxes) return;
            Settings.Instance.SetSetting("exit_explicit", true);
        }

        private void ExitExplicit_Unchecked(object sender, RoutedEventArgs e)
        {
            if (lockCheckboxes) return;
            Settings.Instance.SetSetting("exit_explicit", false);
            StartHided.IsChecked = false;
        }

        #endregion
    }
}
