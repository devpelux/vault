using System;
using System.Collections.Generic;

namespace Vault.Core.Settings
{
    /// <summary>
    /// Settings singleton that manages loading and saving the current session settings for the user.
    /// </summary>
    public sealed class SessionSettings : IDisposable
    {
        private readonly Dictionary<string, string> settings = new();

        #region Instance

        private static SessionSettings? _instance;

        /// <summary>
        /// Gets the instance of the settings.
        /// </summary>
        public static SessionSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SessionSettings();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SessionSettings"/>.
        /// </summary>
        private SessionSettings() { }

        #endregion

        /// <summary>
        /// Sets the setting with the specified key to the specified value. (null to remove the setting)
        /// </summary>
        public void SetSetting(string key, object? value)
        {
            if (value != null)
            {
                string setting = value.ToString() ?? string.Empty;

                if (settings.ContainsKey(key)) settings[key] = setting;
                else settings.Add(key, setting);
            }
            else settings.Remove(key);
        }

        /// <summary>
        /// Gets the setting with the specified key, if exists, null otherwise.
        /// </summary>
        public TValue? GetSetting<TValue>(string key)
        {
            if (settings.ContainsKey(key))
            {
                return Utility.ConvertFromString<TValue>(settings[key]);
            }
            return default;
        }

        /// <summary>
        /// Clears the settings.
        /// </summary>
        public void Clear() => settings.Clear();

        /// <summary>
        /// Clears the settings.
        /// </summary>
        public void Dispose() => Clear();
    }
}
