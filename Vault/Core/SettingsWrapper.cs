using Microsoft.Win32;
using Vault.Properties;

namespace Vault.Core
{
    internal static class SettingsWrapper
    {
        internal static bool StartOnStartup
        {
            get
            {
                using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                return key.GetValue(App.Name) != null;
            }
            set
            {
                using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (value) key.SetValue(App.Name, App.FullName);
                else key.DeleteValue(App.Name);
            }
        }

        internal static string User
        {
            get => Settings.Default.User;
            set
            {
                Settings.Default.User = value;
                Settings.Default.Save();
            }
        }

        internal static int SectionToLoad
        {
            get => Settings.Default.SectionToLoad;
            set
            {
                Settings.Default.SectionToLoad = value;
                Settings.Default.Save();
            }
        }

        internal static string DBSavedPassword
        {
            get => Settings.Default.DBSavedPassword;
            set
            {
                Settings.Default.DBSavedPassword = value;
                Settings.Default.Save();
            }
        }

        internal static string DBPath
        {
            get => Settings.Default.DBPath;
            set
            {
                Settings.Default.DBPath = value;
                Settings.Default.Save();
            }
        }

        internal static bool UseCustomDBPath
        {
            get => Settings.Default.UseCustomDBPath;
            set
            {
                Settings.Default.UseCustomDBPath = value;
                Settings.Default.Save();
            }
        }

        internal static bool StartHided
        {
            get => Settings.Default.StartHided;
            set
            {
                Settings.Default.StartHided = value;
                Settings.Default.Save();
            }
        }

        internal static void SaveAll() => Settings.Default.Save();
    }
}
