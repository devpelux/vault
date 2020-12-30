using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Vault.Core;

namespace Vault.CustomControls
{
    public record CategoryValues(Category Category, ICollection Values);

    public class VaultTreeView : Control
    {
        private ItemsControl itemsList;

        public List<CategoryValues> ItemsSource
        {
            get { return (List<CategoryValues>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(List<CategoryValues>), typeof(VaultTreeView), new PropertyMetadata(null, OnItemsSourceChanged));

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

        public event EventHandler<bool> ItemExpandedChanged;


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

        private void LoadItems(List<CategoryValues> itemsSource)
        {
            if (itemsList != null)
            {
                itemsList.Items.Clear();
                if (itemsSource != null)
                {
                    foreach (CategoryValues item in itemsSource)
                    {
                        if (item.Values.Count > 0)
                        {
                            VaultTreeViewItem itemViewer = new VaultTreeViewItem
                            {
                                Category = item.Category,
                                Header = item.Category.Label,
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
            => ((VaultTreeView)d).LoadItems((List<CategoryValues>)e.NewValue);

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((VaultTreeView)d).LoadItems((List<CategoryValues>)d.GetValue(ItemsSourceProperty));

        private static void OnItemPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((VaultTreeView)d).LoadItems((List<CategoryValues>)d.GetValue(ItemsSourceProperty));
    }
}
