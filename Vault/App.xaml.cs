using System.IO;
using System.Windows;
using Vault.Controls;
using Vault.Core.Database;
using Vault.Core.Settings;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the current executing directory.
        /// </summary>
        public static string CurrentDirectory { get; } = SystemUtils.GetExecutingDirectory().FullName;

        /// <summary>
        /// Gets the current executing file.
        /// </summary>
        public static string CurrentExecutable { get; } = SystemUtils.GetExecutingFile().FullName;

        /// <summary>
        /// Gets the database path for the specified username.
        /// </summary>
        public static string GetDBPath(string username) => Path.Combine(CurrentDirectory, username + ".db");

        #region Startup and exit

        /// <summary>
        /// Called when the application is started.
        /// Initializes the tray icon and eventually starts the credentials window.
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SQLitePCL.Batteries_V2.Init();

            TrayIcon.LoadInstance();

            if (Settings.Instance.GetSetting<bool>("start_hided")) TrayIcon.Instance.WindowToShow = nameof(CredentialsWindow);
            else new CredentialsWindow(CredentialsWindow.Action.Login).Show();
        }

        /// <summary>
        /// Called when the application is terminated.
        /// Disposes all the resources.
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            DB.Instance.Dispose();
            TrayIcon.Instance.Dispose();
            Settings.Instance.Dispose();
            SessionSettings.Instance.Dispose();
        }

        #endregion
    }
}
