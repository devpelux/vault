using FullControls.Extra;
using System.IO;
using System.Windows;
using Vault.Core;

namespace Vault
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal const string Name = "Vault";
        internal const string FileName = "Vault.exe";
        internal static readonly string Directory = Tools.GetExecutingDirectory().FullName;
        internal static readonly string FullName = Path.Combine(Directory, FileName);


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SQLitePCL.Batteries_V2.Init();
            Settings.Instance.DBPath = GetDBPath();

            if (File.Exists(Settings.Instance.DBPath))
            {
                if (Settings.Instance.DBPassword != null)
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
            Settings.Instance.Dispose();
        }

        private static void OpenDatabaseWithSavedPassword()
        {
            VaultDB.Context = new VaultDBContext(Settings.Instance.DBPath, Settings.Instance.DBPassword);
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
                new MasterPasswordWindow(true).Show();
            }
        }

        private string GetDBPath()
        {
            return Path.Combine(Directory, @"Vault.db");
        }
    }
}
