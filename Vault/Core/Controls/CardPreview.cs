using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vault.Core.Controls
{
    public class CardPreview : Control
    {
        public Brush BackgroundOnMouseOver
        {
            get => (Brush)GetValue(BackgroundOnMouseOverProperty);
            set => SetValue(BackgroundOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(BackgroundOnMouseOver), typeof(Brush), typeof(CardPreview));

        public Brush BorderBrushOnMouseOver
        {
            get => (Brush)GetValue(BorderBrushOnMouseOverProperty);
            set => SetValue(BorderBrushOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnMouseOverProperty =
            DependencyProperty.Register(nameof(BorderBrushOnMouseOver), typeof(Brush), typeof(CardPreview));

        public int ID
        {
            get => (int)GetValue(IDProperty);
            set => SetValue(IDProperty, value);
        }

        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register(nameof(ID), typeof(int), typeof(CardPreview), new PropertyMetadata(-1));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(CardPreview));

        public string Type
        {
            get => (string)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(string), typeof(CardPreview));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CardPreview));

        public Brush LabelForeground
        {
            get => (Brush)GetValue(LabelForegroundProperty);
            set => SetValue(LabelForegroundProperty, value);
        }

        public static readonly DependencyProperty LabelForegroundProperty =
            DependencyProperty.Register(nameof(LabelForeground), typeof(Brush), typeof(CardPreview));

        public Brush TypeForeground
        {
            get => (Brush)GetValue(TypeForegroundProperty);
            set => SetValue(TypeForegroundProperty, value);
        }

        public static readonly DependencyProperty TypeForegroundProperty =
            DependencyProperty.Register(nameof(TypeForeground), typeof(Brush), typeof(CardPreview));

        public double LabelFontSize
        {
            get => (double)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register(nameof(LabelFontSize), typeof(double), typeof(CardPreview));

        public double TypeFontSize
        {
            get => (double)GetValue(TypeFontSizeProperty);
            set => SetValue(TypeFontSizeProperty, value);
        }

        public static readonly DependencyProperty TypeFontSizeProperty =
            DependencyProperty.Register(nameof(TypeFontSize), typeof(double), typeof(CardPreview));


        static CardPreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardPreview), new FrameworkPropertyMetadata(typeof(CardPreview)));
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
