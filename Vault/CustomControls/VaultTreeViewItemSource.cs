using System.Collections;

namespace Vault.CustomControls
{
    public class VaultTreeViewItemSource
    {
        public string Header { get; set; }

        public bool IsExpanded { get; set; }

        public ICollection Values { get; set; }


        public VaultTreeViewItemSource(string header, bool isExpanded, ICollection values)
        {
            Header = header;
            IsExpanded = isExpanded;
            Values = values;
        }

        public VaultTreeViewItemSource(string header, ICollection values) : this(header, true, values) { }
    }
}
