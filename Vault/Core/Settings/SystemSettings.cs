using Microsoft.Win32;

namespace Vault.Core.Settings
{
    /// <summary>
    /// Settings static class that manages loading and saving settings from the system registry.
    /// </summary>
    public static class SystemSettings
    {
        /// <summary>
        /// Gets or sets the setting indicating if the application is setup to start on startup.
        /// Returns null if this setting is already used from another application.
        /// </summary>
        public static bool? StartOnStartup
        {
            get
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null && key.GetValue(App.AppName) is string skey)
                {
                    return skey == App.CurrentExecutable ? true : null;
                }
                return false;
            }
            set
            {
                if (value.HasValue)
                {
                    using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (key != null)
                    {
                        //If the value is true, then sets the registry key to enable the autorun, otherwise the key is deleted.
                        if (value.Value) key.SetValue(App.AppName, App.CurrentExecutable);
                        else key.DeleteValue(App.AppName);
                    }
                }
            }
        }
    }
}
