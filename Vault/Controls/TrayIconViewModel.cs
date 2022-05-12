using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vault.Core;
using Vault.Core.Settings;

namespace Vault.Controls
{
    public class TrayIconViewModel : INotifyPropertyChanged
    {
        private VaultStatus _vaultStatus;

        #region Commands

        public ICommand TrayActionShow { get; }
        public ICommand TrayActionLogout { get; }
        public ICommand TrayActionExit { get; }

        #endregion

        public Home HomeWindow { get; set; }

        public string WindowToShow { get; set; }

        #region Icon

        public ImageSource LockedIcon { get; set; }
        public ImageSource UnlockedIcon { get; set; }

        public ImageSource ActualIcon { get; private set; }

        #endregion

        #region ContextMenu

        public ContextMenu LockedContextMenu { get; set; }
        public ContextMenu UnlockedContextMenu { get; set; }

        public ContextMenu ActualContextMenu { get; private set; }

        #endregion

        #region ToolTipText

        public string LockedToolTipText { get; set; }
        public string UnlockedToolTipText { get; set; }

        public string ActualToolTipText { get; private set; }

        #endregion

        public VaultStatus VaultStatus
        {
            get => _vaultStatus;
            set
            {
                _vaultStatus = value;

                ActualIcon = value == VaultStatus.Unlocked ? UnlockedIcon : LockedIcon;
                ActualContextMenu = value == VaultStatus.Unlocked ? UnlockedContextMenu : LockedContextMenu;
                ActualToolTipText = value == VaultStatus.Unlocked ? UnlockedToolTipText : LockedToolTipText;

                OnPropertyChanged(nameof(VaultStatus));
                OnPropertyChanged(nameof(ActualIcon));
                OnPropertyChanged(nameof(ActualContextMenu));
                OnPropertyChanged(nameof(ActualToolTipText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public TrayIconViewModel()
        {
            TrayActionShow = new DelegatedCommand(TrayActionShow_Executed, TrayActionShow_CanExecute);
            TrayActionLogout = new DelegatedCommand(TrayActionLogout_Executed, TrayActionLogout_CanExecute);
            TrayActionExit = new DelegatedCommand(TrayActionExit_Executed, TrayActionExit_CanExecute);
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool TrayActionShow_CanExecute(object obj)
        {
            return WindowToShow != null && WindowToShow != "";
        }

        private void TrayActionShow_Executed(object obj)
        {
            CreateWindow(WindowToShow).Show();
        }

        private bool TrayActionLogout_CanExecute(object obj)
        {
            return VaultStatus == VaultStatus.Unlocked;
        }

        private void TrayActionLogout_Executed(object obj)
        {
            SessionSettings.Instance.Clear();
            VaultStatus = VaultStatus.Locked;
            if (HomeWindow != null)
            {
                new CredentialsWindow(CredentialsWindow.Request.Login).Show();
                HomeWindow.Close();
            }
            else WindowToShow = nameof(CredentialsWindow);
        }

        private bool TrayActionExit_CanExecute(object obj)
        {
            return true;
        }

        private void TrayActionExit_Executed(object obj)
        {
            Application.Current.Shutdown();
        }

        private static Window? CreateWindow(string windowName)
        {
            return windowName switch
            {
                nameof(Home) => new Home(),
                nameof(CredentialsWindow) => new CredentialsWindow(CredentialsWindow.Request.Login),
                _ => null,
            };
        }
    }
}
