using FullControls.SystemComponents;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Vault.Controls;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Settings;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Finestra delle credenziali.
    /// </summary>
    public partial class CredentialsWindow : AvalonWindow, IDialog
    {
        private readonly Request CurrentRequest;
        private bool ReauthResult = false;

        /// <summary>
        /// Initializes a new <see cref="CredentialsWindow"/> with the specified request.
        /// </summary>
        public CredentialsWindow(Request request)
        {
            CurrentRequest = request;
            InitializeComponent();

            switch (request)
            {
                case Request.Login:
                    ConfirmPassword.Visibility = Visibility.Collapsed;
                    break;
                case Request.Registration:
                    break;
                case Request.Reauthentication:
                    ConfirmPassword.Visibility = Visibility.Collapsed;
                    Username.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        public object? GetResult() => ReauthResult;

        /// <summary>
        /// Executed when the window is closed.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (CurrentRequest != Request.Reauthentication && !Settings.Instance.GetSetting<bool>("exit_explicit")) Application.Current.Shutdown();
        }

        /// <summary>
        /// Executed when a key is pressed.
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Enter();
        }

        /// <summary>
        /// Executed when the ok button is pressed.
        /// </summary>
        private void Enter_Click(object sender, RoutedEventArgs e) => Enter();

        /// <summary>
        /// Enter action executed if the enter key is pressed or if the "ok" button is pressed.
        /// </summary>
        private void Enter()
        {
            switch (CurrentRequest)
            {
                case Request.Login:
                    Login();
                    break;
                case Request.Registration:
                    Register();
                    break;
                case Request.Reauthentication:
                    Reauthenticate();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Executes the login.
        /// </summary>
        private void Login()
        {
            if (Username.TextLength == 0 && Password.PasswordLength == 0 || Username.Text.Contains(' '))
            {
                _ = new DialogWindow(new MessageWindow("Input non validi!", "Errore", MessageBoxImage.Exclamation)).Show();
                return;
            }

            if (File.Exists(App.GetDBPath(Username.Text)))
            {
                DB.Context = new(App.GetDBPath(Username.Text), Password.Password);

                //Try to load the database instance to initialize it and check if the connection is ok.
                //If the connection fails aborts the launch and displays an error message.
                if (DB.Instance.IsConnected)
                {
                    StartSession();
                }
                else
                {
                    _ = new DialogWindow(new MessageWindow("Username o password errati o file di dati corrotto!", "Errore", MessageBoxImage.Exclamation)).Show();
                }
                return;
            }

            _ = new DialogWindow(new MessageWindow("Username o password errati!", "Errore", MessageBoxImage.Exclamation)).Show();
        }

        /// <summary>
        /// Executes the registration.
        /// </summary>
        private void Register()
        {
            if (Username.TextLength == 0 || Password.PasswordLength == 0 || ConfirmPassword.PasswordLength == 0 || Username.Text.Contains(' '))
            {
                _ = new DialogWindow(new MessageWindow("Input non validi!", "Errore", MessageBoxImage.Exclamation)).Show();
                return;
            }

            if (!Utility.ComparePasswords(Password.SecurePassword, ConfirmPassword.SecurePassword))
            {
                _ = new DialogWindow(new MessageWindow("Le password non corrispondono!", "Errore", MessageBoxImage.Exclamation)).Show();
                return;
            }

            if (File.Exists(App.GetDBPath(Username.Text)))
            {
                _ = new DialogWindow(new MessageWindow("Utente già esistente!", "Errore", MessageBoxImage.Exclamation)).Show();
                return;
            }

            DB.Context = new(App.GetDBPath(Username.Text), Password.Password);

            //Loads the database instance to initialize it.
            _ = DB.Instance;

            StartSession();
        }

        /// <summary>
        /// Executes the registration.
        /// </summary>
        private void Reauthenticate()
        {
            if (Password.PasswordLength == 0)
            {
                _ = new DialogWindow(new MessageWindow("Input non validi!", "Errore", MessageBoxImage.Exclamation)).Show();
                return;
            }

            //Checks the password with the current session password.
            string? sessionPassword = SessionSettings.Instance.GetSetting<string>("password");
            ReauthResult = Password.Password.Equals(sessionPassword);

            //Closes the window if the password is verified, otherwise displays an error message.
            if (ReauthResult) Close();
            else _ = new DialogWindow(new MessageWindow("Password errata!", "Errore", MessageBoxImage.Exclamation)).Show();
        }

        /// <summary>
        /// Starts the new session.
        /// </summary>
        private void StartSession()
        {
            //Save the username and password in the session settings.
            SessionSettings.Instance.SetSetting("username", Username.Text);
            SessionSettings.Instance.SetSetting("password", Password.Password);

            //If the remember button is checked, remembers the username.
            if (Remember.IsChecked == true) Settings.Instance.SetSetting("username", Username.Text);
            else Settings.Instance.SetSetting("username", null);

            //Loads the home window.
            new Home().Show();
            TrayIcon.Instance.VaultStatus = VaultStatus.Unlocked;
            Close();
        }

        /// <summary>
        /// Defines the credentials window request types.
        /// </summary>
        public enum Request 
        {
            /// <summary>
            /// The window will act as a login window.
            /// </summary>
            Login,

            /// <summary>
            /// The window will act as a registration window.
            /// </summary>
            Registration,

            /// <summary>
            /// The window will act as a reauthentication window.
            /// </summary>
            Reauthentication
        }
    }
}
