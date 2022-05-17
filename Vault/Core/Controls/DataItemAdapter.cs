using Vault.Core.Database.Data;

namespace Vault.Core.Controls
{
    /// <summary>
    /// Adapter to adapt a <see cref="Data"/> element to be displayed into a <see cref="DataItem"/>.
    /// </summary>
    public class DataItemAdapter
    {
        private readonly Data data;

        /// <summary>
        /// Gets the id of the the data.
        /// </summary>
        public int Id => data.Id;

        /// <summary>
        /// Gets a value indicating if the data is locked.
        /// </summary>
        public bool IsLocked => data.IsLocked;

        /// <summary>
        /// Gets the header representing the data.
        /// </summary>
        public string Header => data.Header;

        /// <summary>
        /// Gets the sub-header representing the data.
        /// </summary>
        public string SubHeader => data.SubHeader;

        /// <summary>
        /// Gets a value indicating the position of the item.
        /// </summary>
        public ItemPosition Position { get; set; } = ItemPosition.Middle;

        /// <summary>
        /// Initializes a new <see cref="DataItemAdapter"/> with the specified <see cref="Data"/>.
        /// </summary>
        public DataItemAdapter(Data data)
        {
            this.data = data;
        }

        /// <summary>
        /// Represents the position of the item.
        /// </summary>
        public enum ItemPosition
        {
            /// <summary>
            /// First element.
            /// </summary>
            First,

            /// <summary>
            /// Middle element.
            /// </summary>
            Middle,

            /// <summary>
            /// Last element.
            /// </summary>
            Last
        }
    }
}
