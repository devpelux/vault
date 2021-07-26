using Microsoft.Win32;
using Nucs.JsonSettings;
using System;

namespace Vault.Core
{
    public sealed class Settings : IDisposable
    {
        private SettingsBag settings;

        public bool? StartOnStartup
        {
            get
            {
                using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                return key != null ? key.GetValue(App.Name) is string skey ? skey == App.FullName ? true : null : false : null;
            }
            set
            {
                if (value.HasValue)
                {
                    using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (key != null)
                    {
                        if (value.Value) key.SetValue(App.Name, App.FullName);
                        else key.DeleteValue(App.Name);
                    }
                }
            }
        }

#nullable enable

        public string? User
        {
            get => settings[nameof(User)] as string;
            set => settings[nameof(User)] = value;
        }

        public int? SectionToLoad
        {
            get => settings[nameof(SectionToLoad)] as int?;
            set => settings[nameof(SectionToLoad)] = value;
        }

        public string? DBPassword
        {
            get => settings[nameof(DBPassword)] as string;
            set => settings[nameof(DBPassword)] = value;
        }

        public string? DBPath
        {
            get => settings[nameof(DBPath)] as string;
            set => settings[nameof(DBPath)] = value;
        }

        public bool? StartHided
        {
            get => settings[nameof(StartHided)] as bool?;
            set => settings[nameof(StartHided)] = value;
        }

#nullable disable

        #region Instance

        public bool IsValidInstance { get; private set; } = true;

        private static Settings _instance;

        public static Settings Instance
        {
            get
            {
                if (_instance == null || !_instance.IsValidInstance)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }

        #endregion


        private Settings()
        {
            try
            {
                settings = JsonSettings.Load<SettingsBag>("config.json").EnableAutosave();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            Save();
            settings = null;
            IsValidInstance = false;
        }

        public void Save() => settings?.Save();

#nullable enable

        public void SetSetting(string name, object value) => settings[name] = value;

        public object? GetSetting(string name) => settings[name];

#nullable disable
    }
}
