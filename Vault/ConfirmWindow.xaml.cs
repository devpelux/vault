﻿using FullControls.SystemComponents;
using System;
using System.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Vault.Core;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for dialog boxes.
    /// </summary>
    public partial class ConfirmWindow : AvalonWindow, IDialog
    {
        /// <summary>
        /// Result: false = no, true = yes. (default: false)
        /// </summary>
        private bool Result = false;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the icon type.
        /// </summary>
        public MessageBoxImage IconType { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ConfirmWindow"/> with a message, a title, and an icon type.
        /// </summary>
        public ConfirmWindow(string message, string title, MessageBoxImage iconType)
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

        /// <inheritdoc/>
        public object? GetResult() => Result;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the icon and plays the relative sound.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageViewer.Text = Message;
            switch (IconType)
            {
                case MessageBoxImage.Hand:
                    Image.Source = Utility.LoadIconFromUri("pack://application:,,,/Vault;component/Icons/ic_hand.png");
                    SystemSounds.Hand.Play();
                    break;
                case MessageBoxImage.Question:
                    Image.Source = Utility.LoadIconFromUri("pack://application:,,,/Vault;component/Icons/ic_question.png");
                    SystemSounds.Question.Play();
                    break;
                case MessageBoxImage.Exclamation:
                    Image.Source = Utility.LoadIconFromUri("pack://application:,,,/Vault;component/Icons/ic_exclamation.png");
                    SystemSounds.Exclamation.Play();
                    break;
                case MessageBoxImage.Asterisk:
                    Image.Source = Utility.LoadIconFromUri("pack://application:,,,/Vault;component/Icons/ic_asterisk.png");
                    SystemSounds.Asterisk.Play();
                    break;
                default:
                    Image.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// Executed when the yes button is clicked.
        /// Sets the result to true (yes), then closes the window.
        /// </summary>
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        /// <summary>
        /// Executed when the no button is clicked.
        /// Sets the result to false (no), then closes the window.
        /// </summary>
        private void No_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}
