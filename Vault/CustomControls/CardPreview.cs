﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vault.CustomControls
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

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(CardPreview));

        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register(nameof(Category), typeof(string), typeof(CardPreview));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(CardPreview));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CardPreview));

        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register(nameof(TitleForeground), typeof(Brush), typeof(CardPreview));

        public Brush CategoryForeground
        {
            get => (Brush)GetValue(CategoryForegroundProperty);
            set => SetValue(CategoryForegroundProperty, value);
        }

        public static readonly DependencyProperty CategoryForegroundProperty =
            DependencyProperty.Register(nameof(CategoryForeground), typeof(Brush), typeof(CardPreview));

        public Brush DescriptionForeground
        {
            get => (Brush)GetValue(DescriptionForegroundProperty);
            set => SetValue(DescriptionForegroundProperty, value);
        }

        public static readonly DependencyProperty DescriptionForegroundProperty =
            DependencyProperty.Register(nameof(DescriptionForeground), typeof(Brush), typeof(CardPreview));

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(CardPreview));

        public double CategoryFontSize
        {
            get => (double)GetValue(CategoryFontSizeProperty);
            set => SetValue(CategoryFontSizeProperty, value);
        }

        public static readonly DependencyProperty CategoryFontSizeProperty =
            DependencyProperty.Register(nameof(CategoryFontSize), typeof(double), typeof(CardPreview));

        public double DescriptionFontSize
        {
            get => (double)GetValue(DescriptionFontSizeProperty);
            set => SetValue(DescriptionFontSizeProperty, value);
        }

        public static readonly DependencyProperty DescriptionFontSizeProperty =
            DependencyProperty.Register(nameof(DescriptionFontSize), typeof(double), typeof(CardPreview));


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
