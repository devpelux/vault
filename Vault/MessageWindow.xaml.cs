using FullControls.SystemComponents;
using System;
using System.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for dialog boxes.
    /// </summary>
    public partial class MessageWindow : AvalonWindow, IDialog
    {
        private string Message { get; set; }
        private MessageBoxImage IconType { get; set; }

        private object? Result = null;

        /// <summary>
        /// Initializes a new <see cref="MessageWindow"/> with a message, a title, and an icon type.
        /// </summary>
        public MessageWindow(string message, string title, MessageBoxImage iconType)
        {
            InitializeComponent();
            if (message.Length > 45)
            {
                Height = 210;
                Width = 342;
                message = message[..Math.Min(130, message.Length)];
            }
            Message = message;
            IconType = iconType;
            Title = title;
        }

        /// <inheritdoc/>
        public object? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the icon and plays the relative sound.
        /// </summary>
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
                BitmapImage icon = new();
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

        /// <summary>
        /// Executed when the ok button is clicked.
        /// Sets the result to "ok", then closes the window.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Result = "ok";
            Close();
        }
    }
}
