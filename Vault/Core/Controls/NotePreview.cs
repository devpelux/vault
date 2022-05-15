using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vault.Core.Controls
{
    public class NotePreview : Control
    {
        public Brush BackgroundOnMouseOver
        {
            get => (Brush)GetValue(BackgroundOnMouseOverProperty);
            set => SetValue(BackgroundOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(BackgroundOnMouseOver), typeof(Brush), typeof(NotePreview));

        public Brush BorderBrushOnMouseOver
        {
            get => (Brush)GetValue(BorderBrushOnMouseOverProperty);
            set => SetValue(BorderBrushOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnMouseOverProperty =
            DependencyProperty.Register(nameof(BorderBrushOnMouseOver), typeof(Brush), typeof(NotePreview));

        public int ID
        {
            get => (int)GetValue(IDProperty);
            set => SetValue(IDProperty, value);
        }

        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register(nameof(ID), typeof(int), typeof(NotePreview), new PropertyMetadata(-1));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(NotePreview));

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(NotePreview));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(NotePreview));

        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register(nameof(TitleForeground), typeof(Brush), typeof(NotePreview));

        public Brush SubtitleForeground
        {
            get => (Brush)GetValue(SubtitleForegroundProperty);
            set => SetValue(SubtitleForegroundProperty, value);
        }

        public static readonly DependencyProperty SubtitleForegroundProperty =
            DependencyProperty.Register(nameof(SubtitleForeground), typeof(Brush), typeof(NotePreview));

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(NotePreview));

        public double SubtitleFontSize
        {
            get => (double)GetValue(SubtitleFontSizeProperty);
            set => SetValue(SubtitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty SubtitleFontSizeProperty =
            DependencyProperty.Register(nameof(SubtitleFontSize), typeof(double), typeof(NotePreview));


        static NotePreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotePreview), new FrameworkPropertyMetadata(typeof(NotePreview)));
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
