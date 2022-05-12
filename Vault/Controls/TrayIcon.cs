using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using Vault.Core;

namespace Vault.Controls
{
    public sealed class TrayIcon : IDisposable
    {
        private readonly TaskbarIcon trayIcon;
        private TrayIconViewModel trayIconViewModel;

        public string WindowToShow
        {
            get => trayIconViewModel.WindowToShow;
            set => trayIconViewModel.WindowToShow = value;
        }

        public VaultStatus VaultStatus
        {
            get => trayIconViewModel.VaultStatus;
            set => trayIconViewModel.VaultStatus = value;
        }

        public Home HomeWindow
        {
            get => trayIconViewModel.HomeWindow;
            set => trayIconViewModel.HomeWindow = value;
        }

        #region Instance

        public bool IsValidInstance { get; private set; } = true;

        private static TrayIcon _instance;

        public static TrayIcon Instance
        {
            get
            {
                LoadInstance();
                return _instance;
            }
        }

        #endregion


        private TrayIcon()
        {
            trayIcon = (TaskbarIcon)Application.Current.FindResource("TrayIcon");
            trayIconViewModel = (TrayIconViewModel)trayIcon.DataContext;
        }

        public void Dispose()
        {
            trayIcon.Dispose();
            trayIconViewModel = null;
            IsValidInstance = false;
        }

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        public static void DisposeInstance() => _instance?.Dispose();

        public static void LoadInstance()
        {
            if (_instance == null || !_instance.IsValidInstance)
            {
                _instance = new TrayIcon();
            }
        }
    }
}
