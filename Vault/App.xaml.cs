using System;
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
        internal static readonly string Directory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        internal static readonly string FullName = Path.Combine(Directory, FileName);


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SQLitePCL.Batteries_V2.Init();
            LoadDBPath();

            if (File.Exists(SettingsWrapper.DBPath))
            {
                if (SettingsWrapper.DBSavedPassword != "")
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
            SettingsWrapper.SaveAll();
        }

        private static void OpenDatabaseWithSavedPassword()
        {
            VaultDB.Context = new VaultDBContext(SettingsWrapper.DBPath, SettingsWrapper.DBSavedPassword);
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

        private static void LoadDBPath()
        {
            if (!SettingsWrapper.UseCustomDBPath)
            {
                SettingsWrapper.DBPath = Path.Combine(Directory, @"Vault.db");
            }
        }
    }
}
