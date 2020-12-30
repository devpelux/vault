using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Vault.Core;

namespace Vault.CustomControls
{
    public sealed class VaultTreeViewItem : Control, IDisposable
    {
        private ItemsControl ic;
        private double expandedHeight = 0;
        private double collapsedHeight = 0;

        public Category Category { get; set; }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(VaultTreeViewItem), new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(VaultTreeViewItem), new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(VaultTreeViewItem), new PropertyMetadata(null));

        public ItemsPanelTemplate ItemsPanel
        {
            get => (ItemsPanelTemplate)GetValue(ItemsPanelProperty);
            set => SetValue(ItemsPanelProperty, value);
        }

        public static readonly DependencyProperty ItemsPanelProperty =
            DependencyProperty.Register(nameof(ItemsPanel), typeof(ItemsPanelTemplate), typeof(VaultTreeViewItem), new PropertyMetadata(null));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(VaultTreeViewItem),
                new PropertyMetadata(false, new PropertyChangedCallback((d, e) => ((VaultTreeViewItem)d).OnExpandedChanged((bool)e.NewValue))));

        public event EventHandler<bool> ExpandedChanged;


        static VaultTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VaultTreeViewItem), new FrameworkPropertyMetadata(typeof(VaultTreeViewItem)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ic = (ItemsControl)Template.FindName("PART_ItemsViewer", this);
            ic.SizeChanged += Ic_SizeChanged;
            ((TextBlock)Template.FindName("PART_Header", this)).MouseLeftButtonDown += (o, e) => IsExpanded = !IsExpanded;
        }

        private void OnExpandedChanged(bool isExpanded)
        {
            if (ic != null)
            {
                if (IsExpanded) Expand();
                else Collapse();
                Category = Category with { IsExpanded = IsExpanded };
                VaultDB.Instance.Categories.UpdateRecord(Category);
                ExpandedChanged?.Invoke(this, isExpanded);
            }
        }

        private void Expand()
        {
            DoubleAnimation expand = new DoubleAnimation
            {
                From = collapsedHeight,
                To = expandedHeight,
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 400))
            };
            ic.BeginAnimation(MaxHeightProperty, expand);
        }

        private void Collapse()
        {
            DoubleAnimation collapse = new DoubleAnimation
            {
                From = expandedHeight,
                To = collapsedHeight,
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 400))
            };
            ic.BeginAnimation(MaxHeightProperty, collapse);
        }

        private void Ic_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (expandedHeight == 0)
            {
                expandedHeight = ic.ActualHeight;
                collapsedHeight = 0;
                ic.MaxHeight = expandedHeight;
                if (expandedHeight != 0)
                {
                    IsExpanded = Category.IsExpanded;
                }
            }
        }

        public void Dispose()
        {
            if (ic != null) ic.ItemsSource = null;
        }
    }
}
