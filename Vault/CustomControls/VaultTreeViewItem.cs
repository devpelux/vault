using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Vault.CustomControls
{
    public sealed class VaultTreeViewItem : Control, IDisposable
    {
        private ItemsControl ic;
        private double expandedHeight = 0;
        private double collapsedHeight = 0;


        public int Index { get; set; } = -1;

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
                new PropertyMetadata(true, new PropertyChangedCallback((d, e) => ((VaultTreeViewItem)d).OnExpandedChanged((bool)e.NewValue))));

        public TimeSpan AnimationTime
        {
            get => (TimeSpan)GetValue(AnimationTimeProperty);
            set => SetValue(AnimationTimeProperty, value);
        }

        public static readonly DependencyProperty AnimationTimeProperty =
            DependencyProperty.Register(nameof(AnimationTime), typeof(TimeSpan), typeof(VaultTreeViewItem));

        public event EventHandler<ItemExpandedEventArgs> ExpandedChanged;


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
                ExpandedChanged?.Invoke(this, new ItemExpandedEventArgs(Index, isExpanded));
            }
        }

        private void Expand()
        {
            if (AnimationTime > TimeSpan.Zero)
            {
                DoubleAnimation expand = new()
                {
                    From = collapsedHeight,
                    To = expandedHeight,
                    Duration = new Duration(AnimationTime)
                };
                ic.BeginAnimation(MaxHeightProperty, expand);
            }
            else
            {
                ic.MaxHeight = expandedHeight;
            }
        }

        private void Collapse()
        {
            if (AnimationTime > TimeSpan.Zero)
            {
                DoubleAnimation collapse = new()
                {
                    From = expandedHeight,
                    To = collapsedHeight,
                    Duration = new Duration(AnimationTime)
                };
                ic.BeginAnimation(MaxHeightProperty, collapse);
            }
            else
            {
                ic.MaxHeight = collapsedHeight;
            }
        }

        private void SetInitialState(bool isExpanded) => ic.MaxHeight = isExpanded ? expandedHeight : collapsedHeight;

        private void Ic_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (expandedHeight == 0)
            {
                expandedHeight = ic.ActualHeight;
                collapsedHeight = 0;
                SetInitialState(IsExpanded);
            }
        }

        public void Dispose()
        {
            if (ic != null) ic.ItemsSource = null;
        }
    }
}
