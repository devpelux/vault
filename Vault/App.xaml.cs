using FullControls;
using System.IO;
using System.Windows;
using Vault.Core;
using Vault.Properties;

namespace Vault
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SQLitePCL.Batteries_V2.Init();

            if (File.Exists(Path.GetFullPath(Settings.Default.DBPath)))
            {
                if (Settings.Default.DBSavedPassword != "")
                {
                    OpenDatabaseWithSavedPassword();
                }
                else
                {
                    new MasterPasswordWindow().Show();
                }
            }
            else
            {
                new RegisterMasterPasswordWindow().Show();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.Default.Save();
        }

        private static void OpenDatabaseWithSavedPassword()
        {
            VaultDB.Context = new VaultDBContext(Settings.Default.DBPath, Settings.Default.DBSavedPassword);
            if (VaultDB.Initialize())
            {
                if (VaultDB.Instance.Users.Count() > 0)
                {
                    new LoginWindow().Show();
                }
                else
                {
                    new RegisterWindow().Show();
                }
            }
            else
            {
                _ = new DialogWindow(new MessageWindow("La password memorizzata è errata o il file di dati è corrotto!", "Errore", MessageBoxImage.Exclamation)).Show();
            }
        }
    }
}
