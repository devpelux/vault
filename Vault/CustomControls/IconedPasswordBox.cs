using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControls
{
    public sealed class IconedPasswordBox : Control
    {
        private PasswordBox passwordBox = null;
        private TextBlock peek = null;

        public Brush BackgroundOnSelected
        {
            get => (Brush)GetValue(BackgroundOnSelectedProperty);
            set => SetValue(BackgroundOnSelectedProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnSelectedProperty =
            DependencyProperty.Register(nameof(BackgroundOnSelected), typeof(Brush), typeof(IconedPasswordBox));

        public Brush BackgroundOnDisabled
        {
            get => (Brush)GetValue(BackgroundOnDisabledProperty);
            set => SetValue(BackgroundOnDisabledProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnDisabledProperty =
            DependencyProperty.Register(nameof(BackgroundOnDisabled), typeof(Brush), typeof(IconedPasswordBox));

        public Brush BorderBrushOnSelected
        {
            get => (Brush)GetValue(BorderBrushOnSelectedProperty);
            set => SetValue(BorderBrushOnSelectedProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnSelectedProperty =
            DependencyProperty.Register(nameof(BorderBrushOnSelected), typeof(Brush), typeof(IconedPasswordBox));

        public Brush BorderBrushOnDisabled
        {
            get => (Brush)GetValue(BorderBrushOnDisabledProperty);
            set => SetValue(BorderBrushOnDisabledProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnDisabledProperty =
            DependencyProperty.Register(nameof(BorderBrushOnDisabled), typeof(Brush), typeof(IconedPasswordBox));

        public Brush HintForeground
        {
            get => (Brush)GetValue(HintForegroundProperty);
            set => SetValue(HintForegroundProperty, value);
        }

        public static readonly DependencyProperty HintForegroundProperty =
            DependencyProperty.Register(nameof(HintForeground), typeof(Brush), typeof(IconedPasswordBox));

        public Brush PeekForeground
        {
            get => (Brush)GetValue(PeekForegroundProperty);
            set => SetValue(PeekForegroundProperty, value);
        }

        public static readonly DependencyProperty PeekForegroundProperty =
            DependencyProperty.Register(nameof(PeekForeground), typeof(Brush), typeof(IconedPasswordBox));

        public Brush PeekButtonForeground
        {
            get => (Brush)GetValue(PeekButtonForegroundProperty);
            set => SetValue(PeekButtonForegroundProperty, value);
        }

        public static readonly DependencyProperty PeekButtonForegroundProperty =
            DependencyProperty.Register(nameof(PeekButtonForeground), typeof(Brush), typeof(IconedPasswordBox));

        public Brush CaretBrush
        {
            get => (Brush)GetValue(CaretBrushProperty);
            set => SetValue(CaretBrushProperty, value);
        }

        public static readonly DependencyProperty CaretBrushProperty =
            DependencyProperty.Register(nameof(CaretBrush), typeof(Brush), typeof(IconedPasswordBox));

        public Brush SelectionBrush
        {
            get => (Brush)GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(IconedPasswordBox));

        public Brush SelectionTextBrush
        {
            get => (Brush)GetValue(SelectionTextBrushProperty);
            set => SetValue(SelectionTextBrushProperty, value);
        }

        public static readonly DependencyProperty SelectionTextBrushProperty =
            DependencyProperty.Register(nameof(SelectionTextBrush), typeof(Brush), typeof(IconedPasswordBox));

        public bool AdaptForegroundAutomatically
        {
            get => (bool)GetValue(AdaptForegroundAutomaticallyProperty);
            set => SetValue(AdaptForegroundAutomaticallyProperty, value);
        }

        public static readonly DependencyProperty AdaptForegroundAutomaticallyProperty =
            DependencyProperty.Register(nameof(AdaptForegroundAutomatically), typeof(bool), typeof(IconedPasswordBox));

        public bool AdaptHintForegroundAutomatically
        {
            get => (bool)GetValue(AdaptHintForegroundAutomaticallyProperty);
            set => SetValue(AdaptHintForegroundAutomaticallyProperty, value);
        }

        public static readonly DependencyProperty AdaptHintForegroundAutomaticallyProperty =
            DependencyProperty.Register(nameof(AdaptHintForegroundAutomatically), typeof(bool), typeof(IconedPasswordBox));

        public bool AdaptPeekForegroundAutomatically
        {
            get => (bool)GetValue(AdaptPeekForegroundAutomaticallyProperty);
            set => SetValue(AdaptPeekForegroundAutomaticallyProperty, value);
        }

        public static readonly DependencyProperty AdaptPeekForegroundAutomaticallyProperty =
            DependencyProperty.Register(nameof(AdaptPeekForegroundAutomatically), typeof(bool), typeof(IconedPasswordBox));

        public bool AdaptPeekButtonForegroundAutomatically
        {
            get => (bool)GetValue(AdaptPeekButtonForegroundAutomaticallyProperty);
            set => SetValue(AdaptPeekButtonForegroundAutomaticallyProperty, value);
        }

        public static readonly DependencyProperty AdaptPeekButtonForegroundAutomaticallyProperty =
            DependencyProperty.Register(nameof(AdaptPeekButtonForegroundAutomatically), typeof(bool), typeof(IconedPasswordBox));

        public bool AdaptCaretBrushAutomatically
        {
            get => (bool)GetValue(AdaptCaretBrushAutomaticallyProperty);
            set => SetValue(AdaptCaretBrushAutomaticallyProperty, value);
        }

        public static readonly DependencyProperty AdaptCaretBrushAutomaticallyProperty =
            DependencyProperty.Register(nameof(AdaptCaretBrushAutomatically), typeof(bool), typeof(IconedPasswordBox));

        public double SelectionOpacity
        {
            get => (double)GetValue(SelectionOpacityProperty);
            set => SetValue(SelectionOpacityProperty, value);
        }

        public static readonly DependencyProperty SelectionOpacityProperty =
            DependencyProperty.Register(nameof(SelectionOpacity), typeof(double), typeof(IconedPasswordBox));

        public bool ShowHint
        {
            get => (bool)GetValue(ShowHintProperty);
            set => SetValue(ShowHintProperty, value);
        }

        public static readonly DependencyProperty ShowHintProperty =
            DependencyProperty.Register(nameof(ShowHint), typeof(bool), typeof(IconedPasswordBox));

        public double HintOpacity
        {
            get => (double)GetValue(HintOpacityProperty);
            set => SetValue(HintOpacityProperty, value);
        }

        public static readonly DependencyProperty HintOpacityProperty =
            DependencyProperty.Register(nameof(HintOpacity), typeof(double), typeof(IconedPasswordBox));

        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(nameof(Hint), typeof(string), typeof(IconedPasswordBox));

        public bool EnablePeekButton
        {
            get => (bool)GetValue(EnablePeekButtonProperty);
            set => SetValue(EnablePeekButtonProperty, value);
        }

        public static readonly DependencyProperty EnablePeekButtonProperty =
            DependencyProperty.Register(nameof(EnablePeekButton), typeof(bool), typeof(IconedPasswordBox));

        public double PeekButtonSize
        {
            get => (double)GetValue(PeekButtonSizeProperty);
            set => SetValue(PeekButtonSizeProperty, value);
        }

        public static readonly DependencyProperty PeekButtonSizeProperty =
            DependencyProperty.Register(nameof(PeekButtonSize), typeof(double), typeof(IconedPasswordBox));

        public FontFamily PeekButtonFontFamily
        {
            get => (FontFamily)GetValue(PeekButtonFontFamilyProperty);
            set => SetValue(PeekButtonFontFamilyProperty, value);
        }

        public static readonly DependencyProperty PeekButtonFontFamilyProperty =
            DependencyProperty.Register(nameof(PeekButtonFontFamily), typeof(FontFamily), typeof(IconedPasswordBox));

        public double PeekButtonFontSize
        {
            get => (double)GetValue(PeekButtonFontSizeProperty);
            set => SetValue(PeekButtonFontSizeProperty, value);
        }

        public static readonly DependencyProperty PeekButtonFontSizeProperty =
            DependencyProperty.Register(nameof(PeekButtonFontSize), typeof(double), typeof(IconedPasswordBox));

        public FontStretch PeekButtonFontStretch
        {
            get => (FontStretch)GetValue(PeekButtonFontStretchProperty);
            set => SetValue(PeekButtonFontStretchProperty, value);
        }

        public static readonly DependencyProperty PeekButtonFontStretchProperty =
            DependencyProperty.Register(nameof(PeekButtonFontStretch), typeof(FontStretch), typeof(IconedPasswordBox));

        public FontStyle PeekButtonFontStyle
        {
            get => (FontStyle)GetValue(PeekButtonFontStyleProperty);
            set => SetValue(PeekButtonFontStyleProperty, value);
        }

        public static readonly DependencyProperty PeekButtonFontStyleProperty =
            DependencyProperty.Register(nameof(PeekButtonFontStyle), typeof(FontStyle), typeof(IconedPasswordBox));

        public FontWeight PeekButtonFontWeight
        {
            get => (FontWeight)GetValue(PeekButtonFontWeightProperty);
            set => SetValue(PeekButtonFontWeightProperty, value);
        }

        public static readonly DependencyProperty PeekButtonFontWeightProperty =
            DependencyProperty.Register(nameof(PeekButtonFontWeight), typeof(FontWeight), typeof(IconedPasswordBox));

        public object PeekButtonContent
        {
            get => GetValue(PeekButtonContentProperty);
            set => SetValue(PeekButtonContentProperty, value);
        }

        public static readonly DependencyProperty PeekButtonContentProperty =
            DependencyProperty.Register(nameof(PeekButtonContent), typeof(object), typeof(IconedPasswordBox));

        public bool ShowIcon
        {
            get => (bool)GetValue(ShowIconProperty);
            set => SetValue(ShowIconProperty, value);
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register(nameof(ShowIcon), typeof(bool), typeof(IconedPasswordBox));

        public double MaxIconSize
        {
            get => (double)GetValue(MaxIconSizeProperty);
            set => SetValue(MaxIconSizeProperty, value);
        }

        public static readonly DependencyProperty MaxIconSizeProperty =
            DependencyProperty.Register(nameof(MaxIconSize), typeof(double), typeof(IconedPasswordBox));

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(IconedPasswordBox));

        public HorizontalAlignment TextAlignment
        {
            get => (HorizontalAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(HorizontalAlignment), typeof(IconedPasswordBox));

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(nameof(PasswordChar), typeof(char), typeof(IconedPasswordBox));

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(IconedPasswordBox));

        public int PasswordLength => GetPassword().Length;

        public bool IsSelectionActive => passwordBox.IsSelectionActive;

        public bool IsInactiveSelectionHighlightEnabled
        {
            get => (bool)GetValue(IsInactiveSelectionHighlightEnabledProperty);
            set => SetValue(IsInactiveSelectionHighlightEnabledProperty, value);
        }

        public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty =
            DependencyProperty.Register(nameof(IsInactiveSelectionHighlightEnabled), typeof(bool), typeof(IconedPasswordBox));

        public bool IsPasswordBoxFocused => passwordBox.IsFocused;

        public event RoutedEventHandler PasswordChanged;


        static IconedPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconedPasswordBox), new FrameworkPropertyMetadata(typeof(IconedPasswordBox)));
        }

        public string GetPassword() => passwordBox.Password;

        public void SetPassword(string value) => passwordBox.Password = value;

        public SecureString GetSecurePassword() => passwordBox.SecurePassword;

        public void Clear() => passwordBox.Clear();

        public void Paste() => passwordBox.Paste();

        public void SelectAll() => passwordBox.SelectAll();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            passwordBox = (PasswordBox)Template.FindName("PART_PasswordBox", this);
            peek = (TextBlock)Template.FindName("PART_Peek", this);
            Grid peekButton = (Grid)Template.FindName("PART_PeekButton", this);
            peekButton.MouseLeftButtonDown += PeekButton_MouseLeftButtonDown;
            peekButton.MouseLeftButtonUp += PeekButton_MouseLeftButtonUp;
            peekButton.MouseLeave += PeekButton_MouseLeave;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            passwordBox.GotFocus += PasswordBox_GotFocus;
            passwordBox.LostFocus += PasswordBox_LostFocus;
            IsEnabledChanged += IconedPasswordBox_IsEnabledChanged;
            UpdateHintState();
            AdaptForeColors(Background);
        }

        private void IconedPasswordBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AdaptForeColors((bool)e.NewValue ? BackgroundOnDisabled : Background);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateHintState();
            PasswordChanged?.Invoke(sender, e);
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateHintState();
            _ = VisualStateManager.GoToState(this, "Focused", true);
            AdaptForeColors(BackgroundOnSelected);
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateHintState();
            _ = VisualStateManager.GoToState(this, IsMouseOver ? "MouseOver" : "Normal", true);
            AdaptForeColors(IsMouseOver ? BackgroundOnSelected : Background);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (!IsPasswordBoxFocused)
            {
                _ = VisualStateManager.GoToState(this, "MouseOver", true);
                AdaptForeColors(BackgroundOnSelected);
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!IsPasswordBoxFocused)
            {
                _ = VisualStateManager.GoToState(this, "Normal", true);
                AdaptForeColors(Background);
            }
        }

        private void PeekButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            peek.Text = GetPassword();
            _ = VisualStateManager.GoToState(this, "Peeked", true);
        }

        private void PeekButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            peek.Text = "";
            _ = VisualStateManager.GoToState(this, "Unpeeked", true);
        }

        private void PeekButton_MouseLeave(object sender, MouseEventArgs e)
        {
            peek.Text = "";
            _ = VisualStateManager.GoToState(this, "Unpeeked", true);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            passwordBox.Focus();
        }

        private void UpdateHintState()
        {
            _ = VisualStateManager.GoToState(this, PasswordLength == 0 && !IsPasswordBoxFocused && ShowHint ? "Hinted" : "Unhinted", true);
        }

        private void AdaptForeColors(Brush backgroundBrush)
        {
            if (backgroundBrush is SolidColorBrush brush)
            {
                SolidColorBrush inverseBrush = new SolidColorBrush(GetInverseColor(brush.Color));
                if (AdaptForegroundAutomatically) Foreground = inverseBrush;
                if (AdaptHintForegroundAutomatically) HintForeground = inverseBrush;
                if (AdaptPeekForegroundAutomatically) PeekForeground = inverseBrush;
                if (AdaptPeekButtonForegroundAutomatically) PeekButtonForeground = inverseBrush;
                if (AdaptCaretBrushAutomatically) CaretBrush = inverseBrush;
            }
        }

        private Color GetInverseColor(Color color)
        {
            return Color.FromRgb((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
        }
    }
}
