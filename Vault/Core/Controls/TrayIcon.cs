using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using Vault.Core.Settings;

namespace Vault.Core.Controls
{
    /// <summary>
    /// Tray icon singleton that manages showing and hiding the tray icon, and some icon features.
    /// </summary>
    public sealed class TrayIcon : IDisposable
    {
        private TaskbarIcon? taskbarIcon;
        private TrayIconDC? taskbarIconDC;

        #region Instance

        private static TrayIcon? _instance;

        /// <summary>
        /// Gets the instance of the <see cref="TrayIcon"/> manager.
        /// </summary>
        public static TrayIcon Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TrayIcon();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayIcon"/> manager.
        /// </summary>
        private TrayIcon() { }

        #endregion

        /// <summary>
        /// Checks if the icon is actually displayed.
        /// </summary>
        public bool IsDisplayed => taskbarIcon != null;

        /// <summary>
        /// Occurs when the logout command is executed.
        /// </summary>
        public event EventHandler? LogoutCommandExecuted;

        /// <summary>
        /// Sets the tray icon type.
        /// </summary>
        public void SetIconType(TrayIconType iconType)
        {
            if (taskbarIconDC != null) taskbarIconDC.IconType = iconType;
        }

        /// <summary>
        /// Displays a new instance of the tray icon.
        /// </summary>
        public void Show()
        {
            if (!IsDisplayed)
            {
                taskbarIcon = (TaskbarIcon)Application.Current.FindResource("TrayIcon");
                taskbarIconDC = (TrayIconDC)taskbarIcon.DataContext;
                taskbarIconDC.ShowCommandExecuted -= OnShowCommandExecuted;
                taskbarIconDC.ExitCommandExecuted -= OnExitCommandExecuted;
                taskbarIconDC.LogoutCommandExecuted -= OnLogoutCommandExecuted;
                taskbarIconDC.ShowCommandExecuted += OnShowCommandExecuted;
                taskbarIconDC.ExitCommandExecuted += OnExitCommandExecuted;
                taskbarIconDC.LogoutCommandExecuted += OnLogoutCommandExecuted;
            }
        }

        /// <summary>
        /// Hides and disposes the tray icon.
        /// </summary>
        public void Hide()
        {
            taskbarIcon?.Dispose();
            taskbarIcon = null;
            taskbarIconDC = null;
        }

        /// <summary>
        /// Executed when the show command is executed.
        /// </summary>
        private void OnShowCommandExecuted(object? sender, EventArgs e)
        {
            ShowLastWindow();
        }

        /// <summary>
        /// Executed when the exit command is executed.
        /// </summary>
        private void OnExitCommandExecuted(object? sender, EventArgs e)
        {
            App.RequestShutDown(this);
        }

        /// <summary>
        /// Executed when the logout command is executed.
        /// </summary>
        private void OnLogoutCommandExecuted(object? sender, EventArgs e)
        {
            SetIconType(TrayIconType.Locked);
            if (App.IsHided)
            {
                InstanceSettings.Instance.SetSetting("last_window", nameof(CredentialsWindow));

                //Terminates the current session.
                App.TerminateSession();
            }
            LogoutCommandExecuted?.Invoke(this, e);
        }

        /// <summary>
        /// Hides and disposes the tray icon.
        /// </summary>
        public void Dispose() => Hide();

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        public static void DisposeInstance() => _instance?.Dispose();

        /// <summary>
        /// Shows the last window, if there is a last window to show.
        /// Otherwise executes a log out.
        /// </summary>
        private void ShowLastWindow()
        {
            //Gets and removes the "last_window" setting.
            string? lastWindow = InstanceSettings.Instance.GetSetting<string>("last_window");
            InstanceSettings.Instance.SetSetting("last_window", null);

            //If there is a last window, displays that window.
            //If there is no last window, on the default case, executes a log out.
            switch (lastWindow)
            {
                case nameof(HomeWindow):
                    new HomeWindow().Show();
                    break;
                case nameof(CredentialsWindow):
                    new CredentialsWindow(CredentialsWindow.Request.Login).Show();
                    break;
                default:
                    OnLogoutCommandExecuted(this, EventArgs.Empty);
                    break;
            }
        }
    }
}
