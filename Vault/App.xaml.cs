using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Vault.Core.Controls;
using Vault.Core.Database;
using Vault.Core.Settings;
using Vault.Properties;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Start point.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the application name.
        /// </summary>
        public static string AppName { get; } = "Vault";

        /// <summary>
        /// Gets the current executing directory.
        /// </summary>
        public static string CurrentDirectory { get; } = SystemUtils.GetExecutingDirectory().FullName;

        /// <summary>
        /// Gets the current executing file.
        /// </summary>
        public static string CurrentExecutable { get; } = SystemUtils.GetExecutingFile().FullName;

        /// <summary>
        /// Gets a value indicating if there are no windows displayed.
        /// </summary>
        public static bool IsHided => !Current.Windows.Cast<Window>().Any(w => w.ActualHeight != 0 && w.ActualWidth != 0);

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
            SetCurrentLanguage((Language)Settings.Instance.GetSetting("language", (long)Language.System));

            SQLitePCL.Batteries_V2.Init();

            TrayIcon.Instance.Show();

            CheckAndFixSettings();

            if (Settings.Instance.GetSetting("start_hided", false) && Settings.Instance.GetSetting("exit_explicit", true))
            {
                InstanceSettings.Instance.SetSetting("last_window", nameof(CredentialsWindow));
            }
            else new CredentialsWindow(CredentialsWindow.Request.Login).Show();
        }

        /// <summary>
        /// Called when the application is terminated.
        /// Disposes all the resources.
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            TerminateSession();
            DB.DisposeInstance();
            TrayIcon.DisposeInstance();
            Settings.DisposeInstance();
            InstanceSettings.DisposeInstance();
        }

        #endregion

        /// <summary>
        /// Checks if the "config.json" file is ok and, eventually, resets the file.
        /// </summary>
        private void CheckAndFixSettings()
        {
            if (!Settings.Instance.IsLoaded)
            {
                if (Settings.Instance.Reset())
                {
                    new MessageWindow(Strings.ConfigFixed, Strings.Warning, MessageBoxImage.Exclamation).ShowDialog();
                }
                else
                {
                    new MessageWindow(Strings.ConfigCorrupted, Strings.Error, MessageBoxImage.Exclamation).ShowDialog();
                }
            }
        }

        /// <summary>
        /// Starts a new session.
        /// </summary>
        public static void StartSession(string username, string password)
        {
            //Save the username and password in the session settings.
            InstanceSettings.Instance.SetSetting("username", username);
            InstanceSettings.Instance.SetSetting("password", password);
        }

        /// <summary>
        /// Terminates the current session.
        /// </summary>
        public static void TerminateSession()
        {
            //Removes the username and password in the session settings.
            InstanceSettings.Instance.SetSetting("username", null);
            InstanceSettings.Instance.SetSetting("password", null);
        }

        /// <summary>
        /// Request to shutdown the application.
        /// The application will be closed if is not requested the explicit exit (hide on close and exit by context menu)
        /// and there is no window opened.
        /// </summary>
        public static void RequestShutDown(object sender)
        {
            if (sender is TrayIcon)
            {
                //If the request was sent by the tray icon, then closes the application. (exit explicit)
                Current.Shutdown();
            }
            else if (sender is Window && IsHided)
            {
                //If the request was sent by a window, then checks if can close the application.
                if (Settings.Instance.GetSetting("exit_explicit", true))
                {
                    //If is requested the explicit exit, instead of close the application, saves the window, so it can be opened again.
                    InstanceSettings.Instance.SetSetting("last_window", sender.GetType().Name);
                }
                else Current.Shutdown();
            }
        }

        /// <summary>
        /// Gets the current version number for the application.
        /// </summary>
        public static string GetVersionCode() => typeof(App).Assembly.GetName().Version?.ToString() ?? "N/A";

        /// <summary>
        /// Sets the current application language.
        /// </summary>
        public static void SetCurrentLanguage(Language language)
        {
            CultureInfo.CurrentUICulture = language switch
            {
                Language.enUS => CultureInfo.CreateSpecificCulture("en-US"),
                Language.itIT => CultureInfo.CreateSpecificCulture("it-IT"),
                _ => CultureInfo.InstalledUICulture
            };
        }


        /// <summary>
        /// Represents a language zone.
        /// </summary>
        public enum Language
        {
            /// <summary>
            /// System language.
            /// </summary>
            System = 0,

            /// <summary>
            /// English (United States).
            /// </summary>
            enUS = 1,

            /// <summary>
            /// Italian (Italy).
            /// </summary>
            itIT = 2
        }
    }
}
