using FullControls.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vault.Core.Database.Data;

namespace Vault.Core.Controls
{
    /// <summary>
    /// Displays a preview of a <see cref="Data"/> type with a header and a sub-header.
    /// </summary>
    public class DataItem : Control, IVState
    {
        private bool loaded = false;

        /// <summary>
        /// Gets or sets the bakground brush when the mouse is over the control.
        /// </summary>
        public Brush BackgroundOnMouseOver
        {
            get => (Brush)GetValue(BackgroundOnMouseOverProperty);
            set => SetValue(BackgroundOnMouseOverProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BackgroundOnMouseOver"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(BackgroundOnMouseOver), typeof(Brush), typeof(DataItem));

        /// <summary>
        /// Gets the actual background brush of the control.
        /// </summary>
        public Brush ActualBackground => (Brush)GetValue(ActualBackgroundProperty);

        #region ActualBackgroundProperty

        /// <summary>
        /// The <see cref="DependencyPropertyKey"/> for <see cref="ActualBackground"/> dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey ActualBackgroundPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ActualBackground), typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush)));

        /// <summary>
        /// Identifies the <see cref="ActualBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualBackgroundProperty = ActualBackgroundPropertyKey.DependencyProperty;

        /// <summary>
        /// Proxy for <see cref="ActualBackground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty ActualBackgroundPropertyProxy =
            DependencyProperty.Register("ActualBackgroundProxy", typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush), new PropertyChangedCallback((d, e)
                    => d.SetValue(ActualBackgroundPropertyKey, e.NewValue))));

        #endregion

        /// <summary>
        /// Gets or sets the border brush when the mouse is over the control.
        /// </summary>
        public Brush BorderBrushOnMouseOver
        {
            get => (Brush)GetValue(BorderBrushOnMouseOverProperty);
            set => SetValue(BorderBrushOnMouseOverProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BorderBrushOnMouseOver"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BorderBrushOnMouseOverProperty =
            DependencyProperty.Register(nameof(BorderBrushOnMouseOver), typeof(Brush), typeof(DataItem));

        /// <summary>
        /// Gets the actual border brush of the control.
        /// </summary>
        public Brush ActualBorderBrush => (Brush)GetValue(ActualBorderBrushProperty);

        #region ActualBorderBrushProperty

        /// <summary>
        /// The <see cref="DependencyPropertyKey"/> for <see cref="ActualBorderBrush"/> dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey ActualBorderBrushPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ActualBorderBrush), typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush)));

        /// <summary>
        /// Identifies the <see cref="ActualBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualBorderBrushProperty = ActualBorderBrushPropertyKey.DependencyProperty;

        /// <summary>
        /// Proxy for <see cref="ActualBorderBrush"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty ActualBorderBrushPropertyProxy =
            DependencyProperty.Register("ActualBorderBrushProxy", typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush), new PropertyChangedCallback((d, e)
                    => d.SetValue(ActualBorderBrushPropertyKey, e.NewValue))));

        #endregion

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string? Header
        {
            get => (string?)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(DataItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the sub-header.
        /// </summary>
        public string? SubHeader
        {
            get => (string?)GetValue(SubHeaderProperty);
            set => SetValue(SubHeaderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SubHeader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubHeaderProperty =
            DependencyProperty.Register(nameof(SubHeader), typeof(string), typeof(DataItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the header foreground brush.
        /// </summary>
        public Brush HeaderForeground
        {
            get => (Brush)GetValue(HeaderForegroundProperty);
            set => SetValue(HeaderForegroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(nameof(HeaderForeground), typeof(Brush), typeof(DataItem));

        /// <summary>
        /// Gets or sets the header foreground brush when the mouse is over the control.
        /// </summary>
        public Brush HeaderForegroundOnMouseOver
        {
            get => (Brush)GetValue(HeaderForegroundOnMouseOverProperty);
            set => SetValue(HeaderForegroundOnMouseOverProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderForegroundOnMouseOver"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderForegroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(HeaderForegroundOnMouseOver), typeof(Brush), typeof(DataItem));

        /// <summary>
        /// Gets the actual header foreground brush.
        /// </summary>
        public Brush ActualHeaderForeground => (Brush)GetValue(ActualHeaderForegroundProperty);

        #region ActualHeaderForegroundProperty

        /// <summary>
        /// The <see cref="DependencyPropertyKey"/> for <see cref="ActualHeaderForeground"/> dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey ActualHeaderForegroundPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ActualHeaderForeground), typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush)));

        /// <summary>
        /// Identifies the <see cref="ActualHeaderForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualHeaderForegroundProperty = ActualHeaderForegroundPropertyKey.DependencyProperty;

        /// <summary>
        /// Proxy for <see cref="ActualHeaderForeground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty ActualHeaderForegroundPropertyProxy =
            DependencyProperty.Register("ActualHeaderForegroundProxy", typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush), new PropertyChangedCallback((d, e)
                    => d.SetValue(ActualHeaderForegroundPropertyKey, e.NewValue))));

        #endregion

        /// <summary>
        /// Gets or sets the sub-header foreground brush.
        /// </summary>
        public Brush SubHeaderForeground
        {
            get => (Brush)GetValue(SubHeaderForegroundProperty);
            set => SetValue(SubHeaderForegroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SubHeaderForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubHeaderForegroundProperty =
            DependencyProperty.Register(nameof(SubHeaderForeground), typeof(Brush), typeof(DataItem));

        /// <summary>
        /// Gets or sets the sub-header foreground brush when the mouse is over the control.
        /// </summary>
        public Brush SubHeaderForegroundOnMouseOver
        {
            get => (Brush)GetValue(SubHeaderForegroundOnMouseOverProperty);
            set => SetValue(SubHeaderForegroundOnMouseOverProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SubHeaderForegroundOnMouseOver"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubHeaderForegroundOnMouseOverProperty =
            DependencyProperty.Register(nameof(SubHeaderForegroundOnMouseOver), typeof(Brush), typeof(DataItem));

        /// <summary>
        /// Gets the actual sub-header foreground brush.
        /// </summary>
        public Brush ActualSubHeaderForeground => (Brush)GetValue(ActualSubHeaderForegroundProperty);

        #region ActualSubHeaderForegroundProperty

        /// <summary>
        /// The <see cref="DependencyPropertyKey"/> for <see cref="ActualSubHeaderForeground"/> dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey ActualSubHeaderForegroundPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ActualSubHeaderForeground), typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush)));

        /// <summary>
        /// Identifies the <see cref="ActualSubHeaderForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualSubHeaderForegroundProperty = ActualSubHeaderForegroundPropertyKey.DependencyProperty;

        /// <summary>
        /// Proxy for <see cref="ActualSubHeaderForeground"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty ActualSubHeaderForegroundPropertyProxy =
            DependencyProperty.Register("ActualSubHeaderForegroundProxy", typeof(Brush), typeof(DataItem),
                new FrameworkPropertyMetadata(default(Brush), new PropertyChangedCallback((d, e)
                    => d.SetValue(ActualSubHeaderForegroundPropertyKey, e.NewValue))));

        #endregion

        /// <summary>
        /// Gets or sets the header font size.
        /// </summary>
        public double HeaderFontSize
        {
            get => (double)GetValue(HeaderFontSizeProperty);
            set => SetValue(HeaderFontSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.Register(nameof(HeaderFontSize), typeof(double), typeof(DataItem));

        /// <summary>
        /// Gets or sets the sub-header font size.
        /// </summary>
        public double SubHeaderFontSize
        {
            get => (double)GetValue(SubHeaderFontSizeProperty);
            set => SetValue(SubHeaderFontSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SubHeaderFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubHeaderFontSizeProperty =
            DependencyProperty.Register(nameof(SubHeaderFontSize), typeof(double), typeof(DataItem));

        /// <summary>
        /// Gets or sets the corner radius to set the roundness of the corners.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(DataItem));

        /// <summary>
        /// Gets or sets the duration of the control animation when it changes state.
        /// </summary>
        public TimeSpan AnimationTime
        {
            get => (TimeSpan)GetValue(AnimationTimeProperty);
            set => SetValue(AnimationTimeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="AnimationTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationTimeProperty =
            DependencyProperty.Register(nameof(AnimationTime), typeof(TimeSpan), typeof(DataItem));

        /// <summary>
        /// Gets or sets a value indicating if the displayed data is locked.
        /// </summary>
        public bool IsLocked
        {
            get => (bool)GetValue(IsLockedProperty);
            set => SetValue(IsLockedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register(nameof(IsLocked), typeof(bool), typeof(DataItem));

        /// <summary>
        /// Gets or sets the position of the item.
        /// </summary>
        public DataItemAdapter.ItemPosition Position
        {
            get => (DataItemAdapter.ItemPosition)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(DataItemAdapter.ItemPosition), typeof(DataItem),
                new PropertyMetadata(DataItemAdapter.ItemPosition.Middle));

        static DataItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataItem), new FrameworkPropertyMetadata(typeof(DataItem)));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DataItem"/>.
        /// </summary>
        public DataItem() : base()
        {
            Loaded += (o, e) => OnLoaded(e);
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            loaded = true;
            OnVStateChanged(GetCurrentVState(), true);
        }

        /// <summary>
        /// Called when the element is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnLoaded(RoutedEventArgs e) => OnVStateChanged(GetCurrentVState());

        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            OnVStateChanged(GetCurrentVState());
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            OnVStateChanged(GetCurrentVState());
        }

        /// <inheritdoc/>
        public VState GetCurrentVState()
        {
            if (!loaded) return VState.UNSET;
            else if (IsMouseOver) return VStates.MOUSE_OVER;
            else return VStates.DEFAULT;
        }

        /// <summary>
        /// Called when the current <see cref="VState"/> is changed.
        /// </summary>
        /// <param name="vstate">Current <see cref="VState"/>.</param>
        /// <param name="initial">Specifies if this is the initial <see cref="VState"/>.</param>
        protected virtual void OnVStateChanged(VState vstate, bool initial = false)
        {
            if (vstate == VStates.DEFAULT)
            {
                Utility.AnimateBrush(this, ActualBackgroundPropertyProxy, Background, initial ? TimeSpan.Zero : AnimationTime);
                Utility.AnimateBrush(this, ActualBorderBrushPropertyProxy, BorderBrush, initial ? TimeSpan.Zero : AnimationTime);
                Utility.AnimateBrush(this, ActualHeaderForegroundPropertyProxy, HeaderForeground, initial ? TimeSpan.Zero : AnimationTime);
                Utility.AnimateBrush(this, ActualSubHeaderForegroundPropertyProxy, SubHeaderForeground, initial ? TimeSpan.Zero : AnimationTime);
            }
            else if (vstate == VStates.MOUSE_OVER)
            {
                Utility.AnimateBrush(this, ActualBackgroundPropertyProxy, BackgroundOnMouseOver, initial ? TimeSpan.Zero : AnimationTime);
                Utility.AnimateBrush(this, ActualBorderBrushPropertyProxy, BorderBrushOnMouseOver, initial ? TimeSpan.Zero : AnimationTime);
                Utility.AnimateBrush(this, ActualHeaderForegroundPropertyProxy, HeaderForegroundOnMouseOver, initial ? TimeSpan.Zero : AnimationTime);
                Utility.AnimateBrush(this, ActualSubHeaderForegroundPropertyProxy, SubHeaderForegroundOnMouseOver, initial ? TimeSpan.Zero : AnimationTime);
            }
        }


        /// <summary>
        /// Control v-states.
        /// </summary>
        public static class VStates
        {
            /// <summary>
            /// Default state.
            /// </summary>
            public static readonly VState DEFAULT = new(nameof(DEFAULT), typeof(DataItem));

            /// <summary>
            /// The mouse is over the control.
            /// </summary>
            public static readonly VState MOUSE_OVER = new(nameof(MOUSE_OVER), typeof(DataItem));
        }
    }
}
