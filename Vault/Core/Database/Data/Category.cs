using System;
using System.Collections.Generic;

namespace Vault.Core.Database.Data
{
    /// <summary>
    /// Represents a category with a name.
    /// </summary>
    public class Category : IEquatable<Category?>
    {
        /// <summary>
        /// Represents the default "none" category.
        /// </summary>
        public static Category None { get; } = new("none", string.Empty);

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating if the corrisponding accordion item should be expanded.
        /// </summary>
        public bool IsExpanded { get; set; } = false;

        /// <summary>
        /// Initializes a new <see cref="Category"/>.
        /// </summary>
        public Category(string name, string label, bool isExpanded = false)
        {
            Name = name;
            Label = label;
            IsExpanded = isExpanded;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as Category);

        /// <inheritdoc/>
        public bool Equals(Category? other) => other != null && Name == other.Name;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Name);

        /// <inheritdoc/>
        public static bool operator ==(Category? left, Category? right) => EqualityComparer<Category>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(Category? left, Category? right) => !(left == right);
    }
}
