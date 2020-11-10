using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Vault.CustomControls;

namespace Vault
{
    /// <summary>
    /// Finestra di messaggio.
    /// </summary>
    public partial class MessageWindow : Window, IDialogWindow
    {
        private string Message { get; set; }
        private MessageBoxImage IconType { get; set; }

        private string Result = "";

        public const string NONE = "MessageWindow.NONE";
        public const string OK = "MessageWindow.OK";


        public MessageWindow(string message, string title, MessageBoxImage iconType)
        {
            InitializeComponent();
            if (message.Length > 45)
            {
                Height = 210;
                Width = 342;
                message = message.Substring(0, Math.Min(130, message.Length));
            }
            Message = message;
            IconType = iconType;
            Title = title;
        }

        public string GetResult() => Result;

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Result = NONE;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageViewer.Text = Message;
            string iconUri;
            switch (IconType)
            {
                case MessageBoxImage.Hand:
                    iconUri = "pack://application:,,,/Vault;component/Icons/ic_hand.png";
                    SystemSounds.Hand.Play();
                    break;
                case MessageBoxImage.Question:
                    iconUri = "pack://application:,,,/Vault;component/Icons/ic_question.png";
                    SystemSounds.Question.Play();
                    break;
                case MessageBoxImage.Exclamation:
                    iconUri = "pack://application:,,,/Vault;component/Icons/ic_exclamation.png";
                    SystemSounds.Exclamation.Play();
                    break;
                case MessageBoxImage.Asterisk:
                    iconUri = "pack://application:,,,/Vault;component/Icons/ic_asterisk.png";
                    SystemSounds.Asterisk.Play();
                    break;
                default:
                    iconUri = "";
                    break;
            }
            if (iconUri != "")
            {
                BitmapImage icon = new BitmapImage();
                icon.BeginInit();
                icon.UriSource = new Uri(iconUri);
                icon.EndInit();
                Image.Source = icon;
            }
            else
            {
                Image.Visibility = Visibility.Collapsed;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Result = OK;
            Close();
        }
    }
}
