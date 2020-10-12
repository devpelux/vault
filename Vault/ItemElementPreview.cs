using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControls
{
    public class ItemElementPreview : Control
    {
        public Brush BackgroundOnSelected
        {
            get => (Brush)GetValue(BackgroundOnSelectedProperty);
            set => SetValue(BackgroundOnSelectedProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnSelectedProperty =
            DependencyProperty.Register(nameof(BackgroundOnSelected), typeof(Brush), typeof(ItemElementPreview));

        public Brush BorderBrushOnSelected
        {
            get => (Brush)GetValue(BorderBrushOnSelectedProperty);
            set => SetValue(BorderBrushOnSelectedProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnSelectedProperty =
            DependencyProperty.Register(nameof(BorderBrushOnSelected), typeof(Brush), typeof(ItemElementPreview));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ItemElementPreview));

        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register(nameof(Category), typeof(string), typeof(ItemElementPreview));

        public string Details
        {
            get => (string)GetValue(DetailsProperty);
            set => SetValue(DetailsProperty, value);
        }

        public static readonly DependencyProperty DetailsProperty =
            DependencyProperty.Register(nameof(Details), typeof(string), typeof(ItemElementPreview));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ItemElementPreview));

        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register(nameof(TitleForeground), typeof(Brush), typeof(ItemElementPreview));

        public Brush CategoryForeground
        {
            get => (Brush)GetValue(CategoryForegroundProperty);
            set => SetValue(CategoryForegroundProperty, value);
        }

        public static readonly DependencyProperty CategoryForegroundProperty =
            DependencyProperty.Register(nameof(CategoryForeground), typeof(Brush), typeof(ItemElementPreview));

        public Brush DetailsForeground
        {
            get => (Brush)GetValue(DetailsForegroundProperty);
            set => SetValue(DetailsForegroundProperty, value);
        }

        public static readonly DependencyProperty DetailsForegroundProperty =
            DependencyProperty.Register(nameof(DetailsForeground), typeof(Brush), typeof(ItemElementPreview));

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(ItemElementPreview));

        public double CategoryFontSize
        {
            get => (double)GetValue(CategoryFontSizeProperty);
            set => SetValue(CategoryFontSizeProperty, value);
        }

        public static readonly DependencyProperty CategoryFontSizeProperty =
            DependencyProperty.Register(nameof(CategoryFontSize), typeof(double), typeof(ItemElementPreview));

        public double DetailsFontSize
        {
            get => (double)GetValue(DetailsFontSizeProperty);
            set => SetValue(DetailsFontSizeProperty, value);
        }

        public static readonly DependencyProperty DetailsFontSizeProperty =
            DependencyProperty.Register(nameof(DetailsFontSize), typeof(double), typeof(ItemElementPreview));

        public int ElementID { get; set; } = -1;


        static ItemElementPreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemElementPreview), new FrameworkPropertyMetadata(typeof(ItemElementPreview)));
        }


        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            VisualStateManager.GoToState(this, "MouseOver", true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            VisualStateManager.GoToState(this, "Normal", true);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            VisualStateManager.GoToState(this, "Focused", true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            VisualStateManager.GoToState(this, "Unfocused", true);
        }
    }
}
