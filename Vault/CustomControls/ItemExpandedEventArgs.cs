using System;

namespace Vault.CustomControls
{
    public class ItemExpandedEventArgs : EventArgs
    {
        public int Index { get; set; }

        public bool IsExpanded { get; set; }


        public ItemExpandedEventArgs(int index, bool isExpanded)
        {
            Index = index;
            IsExpanded = isExpanded;
        }
    }
}
