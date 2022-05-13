using System;
using System.Collections.Generic;

namespace Vault.Core.Settings
{
    /// <summary>
    /// Settings singleton that manages loading and saving temporary settings for the current instance of the application.
    /// </summary>
    public sealed class InstanceSettings : IDisposable
    {
        private readonly Dictionary<string, object> settings = new();

        #region Instance

        private static InstanceSettings? _instance;

        /// <summary>
        /// Gets the instance of the settings.
        /// </summary>
        public static InstanceSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InstanceSettings();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InstanceSettings"/>.
        /// </summary>
        private InstanceSettings() { }

        #endregion

        /// <summary>
        /// Sets the setting with the specified key to the specified value.
        /// (Set null to remove the setting)
        /// </summary>
        public void SetSetting(string key, object? value)
        {
            if (value != null)
            {
                if (settings.ContainsKey(key)) settings[key] = value;
                else settings.Add(key, value);
            }
            else settings.Remove(key);
        }

        /// <summary>
        /// Gets the setting with the specified key, if exists, or the specified default value, otherwise.
        /// </summary>
        public TValue? GetSetting<TValue>(string key, TValue? defaultValue = default)
        {
            if (settings.ContainsKey(key) && settings[key] is TValue value)
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Clears the settings.
        /// </summary>
        public void Clear() => settings.Clear();

        /// <summary>
        /// Clears the settings.
        /// </summary>
        public void Dispose() => Clear();

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        public static void DisposeInstance() => _instance?.Dispose();
    }
}
