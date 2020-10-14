using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControls
{
    public class ItemsContainer : Control, ICollection, IEnumerable, IList<Control>
    {
        private Grid container = null;
        private UIElementCollection Children => container?.Children;


        public Control this[int index]
        {
            get => (Control)Children[index];
            set => Children[index] = value;
        }

        public bool IsReadOnly => ((IList)Children).IsReadOnly;

        public int Capacity
        {
            get => Children.Capacity;
            set => Children.Capacity = value;
        }

        public object SyncRoot => Children.SyncRoot;

        public bool IsSynchronized => Children.IsSynchronized;

        public int Count => Children.Count;

        public double Space
        {
            get => (double)GetValue(SpaceProperty);
            set => SetValue(SpaceProperty, value);
        }

        public static readonly DependencyProperty SpaceProperty =
            DependencyProperty.Register(nameof(Space), typeof(double), typeof(ItemsContainer));

        public double OverflowShadowHeight
        {
            get => (double)GetValue(OverflowShadowHeightProperty);
            set => SetValue(OverflowShadowHeightProperty, value);
        }

        public static readonly DependencyProperty OverflowShadowHeightProperty =
            DependencyProperty.Register(nameof(OverflowShadowHeight), typeof(double), typeof(ItemsContainer));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ItemsContainer));


        static ItemsContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemsContainer), new FrameworkPropertyMetadata(typeof(ItemsContainer)));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            container = (Grid)Template.FindName("PART_Container", this);
            ((ScrollViewer)Template.FindName("PART_ScrollViewer", this)).ScrollChanged += OnScrollViewerScrollChanged;
        }

        public int Add(Control element)
        {
            element.HorizontalAlignment = HorizontalAlignment.Stretch;
            element.VerticalAlignment = VerticalAlignment.Top;
            double marginTop = Count == 0 ? 0 : this[Count - 1].Margin.Top + this[Count - 1].Height + Space;
            element.Margin = new Thickness(0, marginTop, 0, 0);
            return Children.Add(element);
        }

        public void Clear() => Children.Clear();

        public bool Contains(Control element) => Children.Contains(element);

        public void CopyTo(Control[] array, int index) => Children.CopyTo(array, index);

        public void CopyTo(Array array, int index) => Children.CopyTo(array, index);

        public IEnumerator GetEnumerator() => Children.GetEnumerator();

        public int IndexOf(Control element) => Children.IndexOf(element);

        public void Insert(int index, Control element)
        {
            Children.Insert(index, element);
            ResetMargins();
        }

        public void Remove(Control element)
        {
            Children.Remove(element);
            ResetMargins();
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
            ResetMargins();
        }

        public void RemoveRange(int index, int count)
        {
            Children.RemoveRange(index, count);
            ResetMargins();
        }


        void ICollection<Control>.Add(Control item) => Add(item);

        bool ICollection<Control>.Remove(Control item)
        {
            if (Contains(item))
            {
                Remove(item);
                return true;
            }
            else return false;
        }

        IEnumerator<Control> IEnumerable<Control>.GetEnumerator() => (IEnumerator<Control>)Children.GetEnumerator();


        private void ResetMargins()
        {
            if (Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    double marginTop = i == 0 ? 0 : this[i - 1].Margin.Top + this[i - 1].Height + Space;
                    this[i].Margin = new Thickness(0, marginTop, 0, 0);
                }
            }
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer viewer = (ScrollViewer)sender;
            if (e.ExtentHeight <= e.ViewportHeight) viewer.OpacityMask = GenerateMask(false, false, OverflowShadowHeight, e.ViewportHeight);
            else
            {
                if (e.VerticalOffset == 0) viewer.OpacityMask = GenerateMask(false, true, OverflowShadowHeight, e.ViewportHeight);
                else if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight) viewer.OpacityMask = GenerateMask(true, false, OverflowShadowHeight, e.ViewportHeight);
                else viewer.OpacityMask = GenerateMask(true, true, OverflowShadowHeight, e.ViewportHeight);
            }
        }

        private LinearGradientBrush GenerateMask(bool topGradient, bool bottomGradient, double gradientHeight, double height)
        {
            if (gradientHeight * 2 > height) gradientHeight = height / 2;
            LinearGradientBrush mask = new LinearGradientBrush();
            mask.StartPoint = new Point(0.5, 0);
            mask.EndPoint = new Point(0.5, 1);
            if (topGradient && bottomGradient)
            {
                mask.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
                mask.GradientStops.Add(new GradientStop(Colors.Black, gradientHeight / height));
                mask.GradientStops.Add(new GradientStop(Colors.Black, 1 - (gradientHeight / height)));
                mask.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            }
            else if (!topGradient && bottomGradient)
            {
                mask.GradientStops.Add(new GradientStop(Colors.Black, 0));
                mask.GradientStops.Add(new GradientStop(Colors.Black, 1 - (gradientHeight / height)));
                mask.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            }
            else if (topGradient && !bottomGradient)
            {
                mask.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
                mask.GradientStops.Add(new GradientStop(Colors.Black, gradientHeight / height));
                mask.GradientStops.Add(new GradientStop(Colors.Black, 1));
            }
            else
            {
                mask.GradientStops.Add(new GradientStop(Colors.Black, 0));
                mask.GradientStops.Add(new GradientStop(Colors.Black, 1));
            }
            return mask;
        }
    }
}
