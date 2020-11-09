using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vault.CustomControls
{
    public class Switcher : Control
    {
        public Brush BackgroundOnMouseOver
        {
            get => (Brush)GetValue(BackgroundOnMouseOverProperty);
            set => SetValue(BackgroundOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(BackgroundOnMouseOver), typeof(Brush), typeof(Switcher));

        public Brush BackgroundOnActivated
        {
            get => (Brush)GetValue(BackgroundOnActivatedProperty);
            set => SetValue(BackgroundOnActivatedProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnActivatedProperty =
            DependencyProperty.Register(nameof(BackgroundOnActivated), typeof(Brush), typeof(Switcher));

        public Brush BackgroundOnDisabled
        {
            get => (Brush)GetValue(BackgroundOnDisabledProperty);
            set => SetValue(BackgroundOnDisabledProperty, value);
        }

        public static readonly DependencyProperty BackgroundOnDisabledProperty =
            DependencyProperty.Register(nameof(BackgroundOnDisabled), typeof(Brush), typeof(Switcher));

        public Brush BorderBrushOnMouseOver
        {
            get => (Brush)GetValue(BorderBrushOnMouseOverProperty);
            set => SetValue(BorderBrushOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnMouseOverProperty =
            DependencyProperty.Register(nameof(BorderBrushOnMouseOver), typeof(Brush), typeof(Switcher));

        public Brush BorderBrushOnActivated
        {
            get => (Brush)GetValue(BorderBrushOnActivatedProperty);
            set => SetValue(BorderBrushOnActivatedProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnActivatedProperty =
            DependencyProperty.Register(nameof(BorderBrushOnActivated), typeof(Brush), typeof(Switcher));

        public Brush BorderBrushOnDisabled
        {
            get => (Brush)GetValue(BorderBrushOnDisabledProperty);
            set => SetValue(BorderBrushOnDisabledProperty, value);
        }

        public static readonly DependencyProperty BorderBrushOnDisabledProperty =
            DependencyProperty.Register(nameof(BorderBrushOnDisabled), typeof(Brush), typeof(Switcher));

        public Brush ForegroundOnMouseOver
        {
            get => (Brush)GetValue(ForegroundOnMouseOverProperty);
            set => SetValue(ForegroundOnMouseOverProperty, value);
        }

        public static readonly DependencyProperty ForegroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(ForegroundOnMouseOver), typeof(Brush), typeof(Switcher));

        public Brush ForegroundOnActivated
        {
            get => (Brush)GetValue(ForegroundOnActivatedProperty);
            set => SetValue(ForegroundOnActivatedProperty, value);
        }

        public static readonly DependencyProperty ForegroundOnActivatedProperty =
            DependencyProperty.Register(nameof(ForegroundOnActivated), typeof(Brush), typeof(Switcher));

        public Brush ForegroundOnDisabled
        {
            get => (Brush)GetValue(ForegroundOnDisabledProperty);
            set => SetValue(ForegroundOnDisabledProperty, value);
        }

        public static readonly DependencyProperty ForegroundOnDisabledProperty =
            DependencyProperty.Register(nameof(ForegroundOnDisabled), typeof(Brush), typeof(Switcher));

        public bool IsActivated
        {
            get => (bool)GetValue(IsActivatedProperty);
            set => SetValue(IsActivatedProperty, value);
        }

        public static readonly DependencyProperty IsActivatedProperty =
            DependencyProperty.Register(nameof(IsActivated), typeof(bool), typeof(Switcher), new PropertyMetadata(false, new PropertyChangedCallback(OnIsActivatedChanged)));

        public bool IsDeactivableByClick
        {
            get => (bool)GetValue(IsDeactivableByClickProperty);
            set => SetValue(IsDeactivableByClickProperty, value);
        }

        public static readonly DependencyProperty IsDeactivableByClickProperty =
            DependencyProperty.Register(nameof(IsDeactivableByClick), typeof(bool), typeof(Switcher), new PropertyMetadata(true));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(Switcher));

        public event EventHandler<SwitcherActivationChangedEventArgs> ActivationChanged;


        static Switcher()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Switcher), new FrameworkPropertyMetadata(typeof(Switcher)));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (!IsActivated) _ = VisualStateManager.GoToState(this, "MouseOver", true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!IsActivated) _ = VisualStateManager.GoToState(this, "Normal", true);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!IsActivated || IsDeactivableByClick) IsActivated = !IsActivated;
        }

        protected virtual void OnActivationChanged(SwitcherActivationChangedEventArgs e)
        {
            _ = VisualStateManager.GoToState(this, IsActivated ? "Normal" : IsMouseOver ? "MouseOver" : "Normal", true);
            ActivationChanged?.Invoke(this, e);
        }

        private static void OnIsActivatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Switcher s = (Switcher)d;
            s.OnActivationChanged(new SwitcherActivationChangedEventArgs(s.IsActivated, s.IsDeactivableByClick));
        }
    }


    public class SwitcherActivationChangedEventArgs : EventArgs
    {
        public bool IsActivated { get; set; }
        public bool IsDeactivableByClick { get; set; }


        public SwitcherActivationChangedEventArgs(bool isActivated, bool isDeactivableByClick)
        {
            IsActivated = isActivated;
            IsDeactivableByClick = isDeactivableByClick;
        }
    }
}
