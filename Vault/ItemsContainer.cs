using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vault
{
    public class ItemsContainer : Control, ICollection, IEnumerable, IList<Control>
    {
        private Grid container = null;
        private UIElementCollection Children => container?.Children;


        public Control this[int index] { get => (Control)Children[index]; set => Children[index] = value; }

        public bool IsReadOnly => ((IList)Children).IsReadOnly;

        public int Capacity { get => Children.Capacity; set => Children.Capacity = value; }

        public object SyncRoot => Children.SyncRoot;

        public bool IsSynchronized => Children.IsSynchronized;

        public int Count => Children.Count;

        public double Space { get => (double)GetValue(SpaceProperty); set => SetValue(SpaceProperty, value); }

        public static readonly DependencyProperty SpaceProperty =
            DependencyProperty.Register(nameof(Space), typeof(double), typeof(ItemsContainer));

        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ItemsContainer));


        static ItemsContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemsContainer), new FrameworkPropertyMetadata(typeof(ItemsContainer)));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            container = (Grid)Template.FindName("container", this);
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
    }
}
