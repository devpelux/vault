using System.Windows;
using System.Windows.Input;

namespace Vault
{
    /// <summary>
    /// Logica di interazione per MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public string Message { get; set; } = "";


        public MessageWindow(string message, string title)
        {
            InitializeComponent();
            Message = message.Substring(0, 50);
            Title = title;
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageViewer.Text = Message;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
