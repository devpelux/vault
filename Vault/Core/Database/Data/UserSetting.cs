namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents an user setting identified by a key and with a string value.
    /// </summary>
    public class UserSetting
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new <see cref="UserSetting"/>.
        /// </summary>
        public UserSetting(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
