using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vault.CustomControls
{
    public class PasswordPreview : Control
    {
        public Brush BackgroundOnMouseOver
        {
            get => (Brush)GetValue(BackgroundOnMouseOverProperty);
            set => SetValue(BackgroundOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(BackgroundOnMouseOver), typeof(Brush), typeof(PasswordPreview));

        public Brush BorderBrushOnMouseOver
        {
            get => (Brush)GetValue(BorderBrushOnMouseOverProperty);
            set => SetValue(BorderBrushOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnMouseOverProperty =
            DependencyProperty.Register(nameof(BorderBrushOnMouseOver), typeof(Brush), typeof(PasswordPreview));

        public int ID
        {
            get => (int)GetValue(IDProperty);
            set => SetValue(IDProperty, value);
        }

        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register(nameof(ID), typeof(int), typeof(PasswordPreview), new PropertyMetadata(-1));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(PasswordPreview));

        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register(nameof(Url), typeof(string), typeof(PasswordPreview));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PasswordPreview));

        public Brush LabelForeground
        {
            get => (Brush)GetValue(LabelForegroundProperty);
            set => SetValue(LabelForegroundProperty, value);
        }

        public static readonly DependencyProperty LabelForegroundProperty =
            DependencyProperty.Register(nameof(LabelForeground), typeof(Brush), typeof(PasswordPreview));

        public Brush UrlForeground
        {
            get => (Brush)GetValue(UrlForegroundProperty);
            set => SetValue(UrlForegroundProperty, value);
        }

        public static readonly DependencyProperty UrlForegroundProperty =
            DependencyProperty.Register(nameof(UrlForeground), typeof(Brush), typeof(PasswordPreview));

        public double LabelFontSize
        {
            get => (double)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register(nameof(LabelFontSize), typeof(double), typeof(PasswordPreview));

        public double UrlFontSize
        {
            get => (double)GetValue(UrlFontSizeProperty);
            set => SetValue(UrlFontSizeProperty, value);
        }

        public static readonly DependencyProperty UrlFontSizeProperty =
            DependencyProperty.Register(nameof(UrlFontSize), typeof(double), typeof(PasswordPreview));


        static PasswordPreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordPreview), new FrameworkPropertyMetadata(typeof(PasswordPreview)));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            _ = VisualStateManager.GoToState(this, "MouseOver", true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _ = VisualStateManager.GoToState(this, "Normal", true);
        }
    }
}
