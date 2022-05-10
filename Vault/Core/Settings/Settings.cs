using Nucs.JsonSettings;
using System;

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
        /// Sets the setting with the specified key to the specified value. (null to remove the setting)
        /// </summary>
        public void SetSetting(string key, object? value)
        {
            if (value != null) settings?.Set(key, value);
            else settings?.Remove(key);
        }

        /// <summary>
        /// Gets the setting with the specified key, if exists, null otherwise.
        /// </summary>
        public TValue? GetSetting<TValue>(string key)
        {
            return settings != null ? settings.Get<TValue>(key) : default;
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
    }
}
