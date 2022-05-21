using System;
using System.ComponentModel;
using System.Windows.Input;
using WpfCoreTools;

namespace Vault.Core.Controls
{
    /// <summary>
    /// Represents the data context for the tray icon.
    /// </summary>
    public class TrayIconDC : INotifyPropertyChanged
    {
        private TrayIconType iconType;

        /// <summary>
        /// Gets or sets the current icon type.
        /// </summary>
        public TrayIconType IconType
        {
            get => iconType;
            set
            {
                if (iconType != value)
                {
                    iconType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IconType)));
                }
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when the show command is executed.
        /// </summary>
        public event EventHandler? ShowCommandExecuted;

        /// <summary>
        /// Occurs when the exit command is executed.
        /// </summary>
        public event EventHandler? ExitCommandExecuted;

        /// <summary>
        /// Occurs when the logout command is executed.
        /// </summary>
        public event EventHandler? LogoutCommandExecuted;

        /// <summary>
        /// Command to show the application, if hidden.
        /// </summary>
        public ICommand TrayCommandShow { get; }

        /// <summary>
        /// Command to close the application.
        /// </summary>
        public ICommand TrayCommandExit { get; }

        /// <summary>
        /// Command to log out.
        /// </summary>
        public ICommand TrayCommandLogout { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="TrayIconDC"/>.
        /// </summary>
        public TrayIconDC()
        {
            IconType = TrayIconType.Locked;
            TrayCommandShow = new DelegateCommand(OnTrayCommandShowExecuted, OnTrayCommandShowCanExecute);
            TrayCommandExit = new DelegateCommand(OnTrayCommandExitExecuted, OnTrayCommandExitCanExecute);
            TrayCommandLogout = new DelegateCommand(OnTrayCommandLogoutExecuted, OnTrayCommandLogoutCanExecute);
        }

        #region Commands handlers

        private bool OnTrayCommandShowCanExecute(object? obj)
        {
            return App.IsHided;
        }

        private bool OnTrayCommandExitCanExecute(object? obj)
        {
            return true;
        }

        private bool OnTrayCommandLogoutCanExecute(object? obj)
        {
            return IconType == TrayIconType.Unlocked;
        }

        private void OnTrayCommandShowExecuted(object? obj)
        {
            ShowCommandExecuted?.Invoke(this, EventArgs.Empty);
        }

        private void OnTrayCommandExitExecuted(object? obj)
        {
            ExitCommandExecuted?.Invoke(this, EventArgs.Empty);
        }

        private void OnTrayCommandLogoutExecuted(object? obj)
        {
            LogoutCommandExecuted?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
