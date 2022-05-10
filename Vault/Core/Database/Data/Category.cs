namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a category with a name.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating if the corrisponding accordion item should be expanded.
        /// </summary>
        public bool IsExpanded { get; set; } = false;

        /// <summary>
        /// Initializes a new <see cref="Category"/>.
        /// </summary>
        public Category(string name, bool isExpanded = false)
        {
            Name = name;
            IsExpanded = isExpanded;
        }
    }
}
