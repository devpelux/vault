using System.Collections.Generic;

namespace Vault.Core.Settings
{
    /// <summary>
    /// Settings singleton that manages loading and saving the current session settings for the user.
    /// </summary>
    public class SessionSettings
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
        /// Sets the setting with the specified key to the specified value.
        /// </summary>
        public void SetSetting(string key, object value)
        {
            settings.Add(key, value.ToString() ?? string.Empty);
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
    }
}
