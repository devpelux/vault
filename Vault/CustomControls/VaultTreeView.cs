using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Vault.CustomControls
{
    public class VaultTreeView : Control
    {
        private ItemsControl itemsList;

        public List<VaultTreeViewItemSource> ItemsSource
        {
            get { return (List<VaultTreeViewItemSource>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(List<VaultTreeViewItemSource>), typeof(VaultTreeView), new PropertyMetadata(null, OnItemsSourceChanged));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(VaultTreeView), new PropertyMetadata(null, OnItemTemplateChanged));

        public ItemsPanelTemplate ItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(ItemsPanelProperty); }
            set { SetValue(ItemsPanelProperty, value); }
        }

        public static readonly DependencyProperty ItemsPanelProperty =
            DependencyProperty.Register(nameof(ItemsPanel), typeof(ItemsPanelTemplate), typeof(VaultTreeView), new PropertyMetadata(null, OnItemPanelChanged));

        public TimeSpan AnimationTime
        {
            get => (TimeSpan)GetValue(AnimationTimeProperty);
            set => SetValue(AnimationTimeProperty, value);
        }

        public static readonly DependencyProperty AnimationTimeProperty =
            DependencyProperty.Register(nameof(AnimationTime), typeof(TimeSpan), typeof(VaultTreeView));

        public event EventHandler<ItemExpandedEventArgs> ItemExpandedChanged;


        static VaultTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VaultTreeView), new FrameworkPropertyMetadata(typeof(VaultTreeView)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            itemsList = (ItemsControl)Template.FindName("PART_ItemsList", this);
            LoadItems(ItemsSource);
        }

        private void LoadItems(List<VaultTreeViewItemSource> itemsSource)
        {
            if (itemsList != null)
            {
                itemsList.Items.Clear();
                if (itemsSource != null)
                {
                    for (int i = 0; i < itemsSource.Count; i++)
                    {
                        VaultTreeViewItemSource item = itemsSource[i];
                        if (item.Values.Count > 0)
                        {
                            VaultTreeViewItem itemViewer = new VaultTreeViewItem
                            {
                                AnimationTime = AnimationTime,
                                Index = i,
                                IsExpanded = item.IsExpanded,
                                Header = item.Header,
                                ItemsSource = item.Values,
                                ItemTemplate = ItemTemplate,
                                ItemsPanel = ItemsPanel
                            };
                            itemViewer.ExpandedChanged += ItemExpandedChanged;
                            _ = itemsList.Items.Add(itemViewer);
                        }
                    }
                }
            }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((VaultTreeView)d).LoadItems((List<VaultTreeViewItemSource>)e.NewValue);

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((VaultTreeView)d).LoadItems((List<VaultTreeViewItemSource>)d.GetValue(ItemsSourceProperty));

        private static void OnItemPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((VaultTreeView)d).LoadItems((List<VaultTreeViewItemSource>)d.GetValue(ItemsSourceProperty));
    }
}
