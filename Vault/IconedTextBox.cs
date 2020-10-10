using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControls
{
    public class IconedTextBox : TextBox
    {
        public Brush BackgroundOnMouseOver
        {
            get => (Brush)GetValue(BackgroundOnMouseOverProperty);
            set => SetValue(BackgroundOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(BackgroundOnMouseOver), typeof(Brush), typeof(IconedTextBox));

        public Brush BackgroundOnFocused
        {
            get => (Brush)GetValue(BackgroundOnFocusedProperty);
            set => SetValue(BackgroundOnFocusedProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnFocusedProperty =
            DependencyProperty.Register(nameof(BackgroundOnFocused), typeof(Brush), typeof(IconedTextBox));

        public Brush BackgroundOnDisabled
        {
            get => (Brush)GetValue(BackgroundOnDisabledProperty);
            set => SetValue(BackgroundOnDisabledProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnDisabledProperty =
            DependencyProperty.Register(nameof(BackgroundOnDisabled), typeof(Brush), typeof(IconedTextBox));

        public Brush BorderBrushOnMouseOver
        {
            get => (Brush)GetValue(BorderBrushOnMouseOverProperty);
            set => SetValue(BorderBrushOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnMouseOverProperty =
            DependencyProperty.Register(nameof(BorderBrushOnMouseOver), typeof(Brush), typeof(IconedTextBox));

        public Brush BorderBrushOnFocused
        {
            get => (Brush)GetValue(BorderBrushOnFocusedProperty);
            set => SetValue(BorderBrushOnFocusedProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnFocusedProperty =
            DependencyProperty.Register(nameof(BorderBrushOnFocused), typeof(Brush), typeof(IconedTextBox));

        public Brush BorderBrushOnDisabled
        {
            get => (Brush)GetValue(BorderBrushOnDisabledProperty);
            set => SetValue(BorderBrushOnDisabledProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnDisabledProperty =
            DependencyProperty.Register(nameof(BorderBrushOnDisabled), typeof(Brush), typeof(IconedTextBox));

        public Brush HintForeground
        {
            get => (Brush)GetValue(HintForegroundProperty);
            set => SetValue(HintForegroundProperty, value);
        }

        public static readonly DependencyProperty HintForegroundProperty =
            DependencyProperty.Register(nameof(HintForeground), typeof(Brush), typeof(IconedTextBox));

        public bool ShowHint
        {
            get => (bool)GetValue(ShowHintProperty);
            set => SetValue(ShowHintProperty, value);
        }

        public static readonly DependencyProperty ShowHintProperty =
            DependencyProperty.Register(nameof(ShowHint), typeof(bool), typeof(IconedTextBox));

        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(nameof(Hint), typeof(string), typeof(IconedTextBox));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(IconedTextBox));

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(IconedTextBox));


        static IconedTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconedTextBox), new FrameworkPropertyMetadata(typeof(IconedTextBox)));
            TextProperty.OverrideMetadata(typeof(IconedTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTextChanged)));
        }


        static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            IconedTextBox tb = (IconedTextBox)sender;
            tb.UpdateHintState();
        }

        private void UpdateHintState()
        {
            if (Text.Length == 0 && !IsFocused && ShowHint) VisualStateManager.GoToState(this, "Hinted", true);
            else VisualStateManager.GoToState(this, "Unhinted", true);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdateHintState();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateHintState();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "Hinted", true);
        }
    }
}
