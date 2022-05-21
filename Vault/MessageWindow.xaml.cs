using FullControls.SystemComponents;
using System;
using System.Media;
using System.Windows;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for dialog boxes.
    /// </summary>
    public partial class MessageWindow : AvalonWindow
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the icon type.
        /// </summary>
        public MessageBoxImage IconType { get; set; }

        /// <summary>
        /// Initializes a new <see cref="MessageWindow"/> with a message, a title, and an icon type.
        /// </summary>
        public MessageWindow(string message, string title, MessageBoxImage iconType)
        {
            InitializeComponent();
            if (message.Length > 45)
            {
                Width = 300;
                message = message[..Math.Min(130, message.Length)];
            }
            Message = message;
            IconType = iconType;
            Title = title;
        }

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the icon and plays the relative sound.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            MessageViewer.Text = Message;
            switch (IconType)
            {
                case MessageBoxImage.Hand:
                    Image.Source = GraphicUtils.LoadBitmapImageFromUri("pack://application:,,,/Vault;component/Icons/ic_hand.png");
                    SystemSounds.Hand.Play();
                    break;
                case MessageBoxImage.Question:
                    Image.Source = GraphicUtils.LoadBitmapImageFromUri("pack://application:,,,/Vault;component/Icons/ic_question.png");
                    SystemSounds.Question.Play();
                    break;
                case MessageBoxImage.Exclamation:
                    Image.Source = GraphicUtils.LoadBitmapImageFromUri("pack://application:,,,/Vault;component/Icons/ic_exclamation.png");
                    SystemSounds.Exclamation.Play();
                    break;
                case MessageBoxImage.Asterisk:
                    Image.Source = GraphicUtils.LoadBitmapImageFromUri("pack://application:,,,/Vault;component/Icons/ic_asterisk.png");
                    SystemSounds.Asterisk.Play();
                    break;
                default:
                    Image.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// Executed when the ok button is clicked.
        /// Closes the window.
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e) => Close();
    }
}
