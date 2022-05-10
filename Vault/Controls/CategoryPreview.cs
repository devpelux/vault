using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vault.Controls
{
    public class CategoryPreview : Control
    {
        public bool EditMode
        {
            get => (bool)GetValue(EditModeProperty);
            set => SetValue(EditModeProperty, value);
        }

        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register(nameof(EditMode), typeof(bool), typeof(CategoryPreview), new PropertyMetadata(false,
                new PropertyChangedCallback((d, e) => ((CategoryPreview)d).OnEditModeChanged(new EditModeChangedEventArgs((bool)e.NewValue)))));

        public int ID
        {
            get => (int)GetValue(IDProperty);
            set => SetValue(IDProperty, value);
        }

        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register(nameof(ID), typeof(int), typeof(CategoryPreview), new PropertyMetadata(-1));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(CategoryPreview));

        public Style ButtonStyle
        {
            get => (Style)GetValue(ButtonStyleProperty);
            set => SetValue(ButtonStyleProperty, value);
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register(nameof(ButtonStyle), typeof(Style), typeof(CategoryPreview));

        public Style TextBoxStyle
        {
            get => (Style)GetValue(TextBoxStyleProperty);
            set => SetValue(TextBoxStyleProperty, value);
        }

        public static readonly DependencyProperty TextBoxStyleProperty =
            DependencyProperty.Register(nameof(TextBoxStyle), typeof(Style), typeof(CategoryPreview));

        public event EventHandler<EditModeChangedEventArgs> EditModeChanged;


        static CategoryPreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CategoryPreview), new FrameworkPropertyMetadata(typeof(CategoryPreview)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TextBox textBox = (TextBox)Template.FindName("TextBox", this);
            ((Button)Template.FindName("Button", this)).Click += (s, e) =>
            {
                EditMode = true;
                textBox.Focus();
            };
            textBox.LostFocus += (s, e) => EditMode = false;
            textBox.KeyDown += (s, e) => EditMode = e.Key != Key.Return;
        }

        protected virtual void OnEditModeChanged(EditModeChangedEventArgs e)
        {
            EditModeChanged?.Invoke(this, e);
        }
    }


    public class EditModeChangedEventArgs : EventArgs
    {
        public bool EditMode { get; init; }


        public EditModeChangedEventArgs(bool editMode) => EditMode = editMode;
    }
}
