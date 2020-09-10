using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vault
{
    public class IconedTextBox : TextBox
    {
        public ImageSource Source { get => (ImageSource)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(IconedTextBox));

        public double IconSize { get => (double)GetValue(IconSizeProperty); set => SetValue(IconSizeProperty, value); }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(IconedTextBox));


        static IconedTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconedTextBox), new FrameworkPropertyMetadata(typeof(IconedTextBox)));
        }
    }
}
