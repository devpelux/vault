using FullControls.SystemComponents;
using System;
using System.Windows;
using System.Windows.Controls;
using Vault.Core.Settings;
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Window for changing the settings.
    /// </summary>
    public partial class SettingsWindow : AvalonWindow
    {
        private bool lockCheckboxes = true;
        private bool lockComboboxes = true;

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
            lockComboboxes = true;

            StartOnStartup.IsChecked = SystemSettings.StartOnStartup == true;
            StartHided.IsChecked = Settings.Instance.GetSetting("start_hided", false);
            ExitExplicit.IsChecked = Settings.Instance.GetSetting("exit_explicit", true);

            //Loads the language setting.
            LoadLanguage();

            lockCheckboxes = false;
            lockComboboxes = false;
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
            else if (new ConfirmWindow(Strings.StartOnStartupUsed, Strings.Warning, MessageBoxImage.Question) { Owner = this }.ShowDialogForResult<bool>())
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

        #region Language

        /// <summary>
        /// Loads the language setting.
        /// </summary>
        private void LoadLanguage()
        {
            App.Language language = (App.Language)Settings.Instance.GetSetting("language", (long)App.Language.System);

            AppLanguage.SelectedIndex = language switch
            {
                App.Language.System => 0,
                App.Language.enUS => 1,
                App.Language.itIT => 2,
                _ => 0,
            };
        }

        private void AppLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lockComboboxes) return;

            App.Language language = AppLanguage.SelectedIndex switch
            {
                0 => App.Language.System,
                1 => App.Language.enUS,
                2 => App.Language.itIT,
                _ => App.Language.System,
            };

            Settings.Instance.SetSetting("language", language);

            new MessageWindow(Strings.LanguageChangedInfo, Strings.Info, MessageBoxImage.Information) { Owner = this }.ShowDialog();
        }

        #endregion
    }
}
