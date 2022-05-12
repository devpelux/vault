using Nucs.JsonSettings;
using System;
using System.IO;

namespace Vault.Core.Settings
{
    /// <summary>
    /// Settings singleton that manages loading and saving settings from a json file named "config.json".
    /// </summary>
    public sealed class Settings : IDisposable
    {
        /// <summary>
        /// Connection to the json settings file.
        /// </summary>
        private SettingsBag? settings;

        #region Instance

        private static Settings? _instance;

        /// <summary>
        /// Gets the instance of the settings.
        /// The instance is reloaded if the settings are not loaded.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (_instance == null || !_instance.IsLoaded)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Settings"/> with a new connection to a settings json file named "config.json".
        /// </summary>
        private Settings()
        {
            try
            {
                settings = JsonSettings.Load<SettingsBag>("config.json").EnableAutosave();
            }
            catch
            {
                settings = null;
            }
        }

        #endregion

        /// <summary>
        /// Checks if the settings are loaded correctly.
        /// </summary>
        public bool IsLoaded => settings != null;

        /// <summary>
        /// Sets the setting with the specified key to the specified value.
        /// (Set null to remove the setting)
        /// </summary>
        public void SetSetting(string key, object? value)
        {
            if (value != null) settings?.Set(key, value);
            else settings?.Remove(key);
        }

        /// <summary>
        /// Gets the setting with the specified key, if exists, or the specified default value, otherwise.
        /// </summary>
        public TValue? GetSetting<TValue>(string key, TValue? defaultValue = default)
        {
            return (TValue?)(settings?.Get<object>(key) ?? defaultValue);
        }

        /// <summary>
        /// Removes all the settings and recreates the configuration file.<br/>
        /// Returns a value indicating if the operation was succesful.
        /// </summary>
        public bool Reset()
        {
            Dispose();
            try
            {
                File.Delete("config.json");
                _instance = new Settings();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save() => settings?.Save();

        /// <summary>
        /// Saves the settings and unload this instance.
        /// </summary>
        public void Dispose()
        {
            Save();
            settings?.Dispose();
            settings = null;
        }

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        public static void DisposeInstance() => _instance?.Dispose();
    }
}
