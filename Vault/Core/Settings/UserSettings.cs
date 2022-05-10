using Vault.Core.Database;
using Vault.Core.Database.Data;

namespace Vault.Core.Settings
{
    /// <summary>
    /// Settings singleton that manages loading and saving user settings from the user specific database.
    /// </summary>
    public class UserSettings
    {
        #region Instance

        private static UserSettings? _instance;

        /// <summary>
        /// Gets the instance of the settings.
        /// </summary>
        public static UserSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSettings();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="UserSettings"/>.
        /// </summary>
        private UserSettings() { }

        #endregion

        /// <summary>
        /// Sets the setting with the specified key to the specified value.
        /// </summary>
        public void SetSetting(string key, object value)
        {
            UserSetting setting = new(key, value.ToString() ?? string.Empty);

            if (DB.Instance.UserSettings.Exists(key)) DB.Instance.UserSettings.Update(setting);
            else DB.Instance.UserSettings.Add(setting);
        }

        /// <summary>
        /// Gets the setting with the specified key, if exists, null otherwise.
        /// </summary>
        public TValue? GetSetting<TValue>(string key)
        {
            UserSetting? setting = DB.Instance.UserSettings.Get(key);

            if (setting != null)
            {
                return Utility.ConvertFromString<TValue>(setting.Value);
            }
            return default;
        }
    }
}
