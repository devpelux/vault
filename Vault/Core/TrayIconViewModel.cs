using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vault.Core
{
    public class TrayIconViewModel : INotifyPropertyChanged
    {
        public Home HomeWindow { get; set; }

        public ICommand TrayActionShow { get; }
        public ICommand TrayActionLogout { get; }
        public ICommand TrayActionExit { get; }

        public string WindowToShow { get; set; }

        public ImageSource LockedIcon { get; set; }
        public ImageSource UnlockedIcon { get; set; }

        public ImageSource ActualIcon { get; private set; }

        public ContextMenu LockedContextMenu { get; set; }
        public ContextMenu UnlockedContextMenu { get; set; }

        public ContextMenu ActualContextMenu { get; private set; }

        private VaultStatus _vaultStatus;

        public event PropertyChangedEventHandler PropertyChanged;

        public VaultStatus VaultStatus
        {
            get => _vaultStatus;
            set
            {
                ActualIcon = value == VaultStatus.Unlocked ? UnlockedIcon : LockedIcon;
                ActualContextMenu = value == VaultStatus.Unlocked ? UnlockedContextMenu : LockedContextMenu;
                _vaultStatus = value;
                OnPropertyChanged(nameof(VaultStatus));
                OnPropertyChanged(nameof(ActualIcon));
                OnPropertyChanged(nameof(ActualContextMenu));
            }
        }


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
            Session.ClearInstance();
            VaultStatus = VaultStatus.Locked;
            if (HomeWindow != null)
            {
                new LoginWindow().Show();
                HomeWindow.Close();
            }
            else WindowToShow = nameof(LoginWindow);
        }

        private bool TrayActionExit_CanExecute(object obj)
        {
            return true;
        }

        private void TrayActionExit_Executed(object obj)
        {
            Application.Current.Shutdown();
        }

        private static Window CreateWindow(string windowName)
        {
            return windowName switch
            {
                nameof(Home) => new Home(),
                nameof(LoginWindow) => new LoginWindow(),
                nameof(MasterPasswordWindow) => new MasterPasswordWindow(),
                _ => null,
            };
        }
    }
}
