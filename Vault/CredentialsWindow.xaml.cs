using FullControls.SystemComponents;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Settings;
using Vault.Properties;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for input the credentials.
    /// </summary>
    public partial class CredentialsWindow : AvalonWindow, IDialog<bool>
    {
        private Request CurrentRequest;

        /// <summary>
        /// ReauthResult: false = failed, true = successful. (default: false)
        /// </summary>
        private bool ReauthResult = false;

        /// <summary>
        /// Initializes a new <see cref="CredentialsWindow"/> with the specified request.
        /// </summary>
        public CredentialsWindow(Request request)
        {
            InitializeComponent();
            CurrentRequest = request;
        }

        /// <summary>
        /// Setups the window for the specified request.
        /// </summary>
        private void Reload()
        {
            switch (CurrentRequest)
            {
                case Request.Login:
                    ConfirmPassword.Visibility = Visibility.Collapsed;
                    Username.Visibility = Visibility.Visible;
                    Remember.Visibility = Visibility.Visible;
                    RegisterLink.Visibility = Visibility.Visible;
                    LoginLink.Visibility = Visibility.Collapsed;
                    Title = Strings.Login;
                    break;
                case Request.Registration:
                    ConfirmPassword.Visibility = Visibility.Visible;
                    Username.Visibility = Visibility.Visible;
                    Remember.Visibility = Visibility.Visible;
                    RegisterLink.Visibility = Visibility.Collapsed;
                    LoginLink.Visibility = Visibility.Visible;
                    Title = Strings.Register;
                    break;
                case Request.Reauthentication:
                    ConfirmPassword.Visibility = Visibility.Collapsed;
                    Username.Visibility = Visibility.Collapsed;
                    Remember.Visibility = Visibility.Collapsed;
                    RegisterLink.Visibility = Visibility.Collapsed;
                    LoginLink.Visibility = Visibility.Collapsed;
                    Title = Strings.ReauthenticateRequest;
                    break;
            }
        }

        /// <inheritdoc/>
        public bool GetResult() => ReauthResult;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the username if is saved, then setups the window.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

            string? username = Settings.Instance.GetSetting("username", string.Empty);
            Username.Text = username;
            Remember.IsChecked = username != string.Empty;
            Reload();
        }

        /// <summary>
        /// Executed when the window is closed.
        /// Request the shutdown if the window is a login or register window.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (CurrentRequest != Request.Reauthentication) App.RequestShutDown(this);
        }

        /// <summary>
        /// Executed when the register label is pressed.
        /// Switches the request to registration, then resetups the window.
        /// </summary>
        private void RegisterLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentRequest = Request.Registration;
            Reload();
        }

        /// <summary>
        /// Executed when the login label is pressed.
        /// Switches the request to login, then resetups the window.
        /// </summary>
        private void LoginLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentRequest = Request.Login;
            Reload();
        }

        /// <summary>
        /// Executed when a key is pressed.
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ExecuteRequest();
        }

        /// <summary>
        /// Executed when the ok button is pressed.
        /// </summary>
        private void Confirm_Click(object sender, RoutedEventArgs e) => ExecuteRequest();

        /// <summary>
        /// Executes the requested request.
        /// </summary>
        private void ExecuteRequest()
        {
            switch (CurrentRequest)
            {
                case Request.Login:
                    LoginRequest();
                    break;
                case Request.Registration:
                    RegisterRequest();
                    break;
                case Request.Reauthentication:
                    ReauthenticateRequest();
                    break;
            }
        }

        /// <summary>
        /// Executes the login.
        /// </summary>
        private void LoginRequest()
        {
            if (Username.TextLength > 0 && Password.PasswordLength > 0 && !Username.Text.Contains(' '))
            {
                if (File.Exists(App.GetDBPath(Username.Text)))
                {
                    DB.Context = new(App.GetDBPath(Username.Text), Password.Password);

                    //Try to load the database instance to initialize it and check if the connection is ok.
                    //If the connection fails aborts the launch and displays an error message.
                    if (DB.Instance.IsConnected)
                    {
                        Login();
                        return;
                    }
                }
            }

            new MessageWindow(Strings.InvalidCredentials, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
        }

        /// <summary>
        /// Executes the registration.
        /// </summary>
        private void RegisterRequest()
        {
            if (Username.TextLength > 0 && Password.PasswordLength > 0 && ConfirmPassword.PasswordLength > 0 && !Username.Text.Contains(' '))
            {
                if (!Utility.ComparePasswords(Password.SecurePassword, ConfirmPassword.SecurePassword))
                {
                    new MessageWindow(Strings.PasswordsNotMatching, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
                    return;
                }

                if (File.Exists(App.GetDBPath(Username.Text)))
                {
                    new MessageWindow(Strings.UserAlreadyExists, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
                    return;
                }

                DB.Context = new(App.GetDBPath(Username.Text), Password.Password);

                //Try to load the database instance to initialize it and check if the connection is ok.
                //If the connection fails aborts the launch and displays an error message.
                if (DB.Instance.IsConnected) Login();
                else new MessageWindow(Strings.CannotCreateDatabase, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
            }
            else new MessageWindow(Strings.InvalidCredentials, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
        }

        /// <summary>
        /// Executes the registration.
        /// </summary>
        private void ReauthenticateRequest()
        {
            if (Password.PasswordLength > 0)
            {
                //Checks the password with the current session password.
                string? sessionPassword = InstanceSettings.Instance.GetSetting<string>("password");
                ReauthResult = Password.Password.Equals(sessionPassword);

                //Closes the window if the password is verified.
                if (ReauthResult)
                {
                    Close();
                    return;
                }
            }

            new MessageWindow(Strings.InvalidPassword, Strings.Error, MessageBoxImage.Exclamation) { Owner = this }.ShowDialog();
        }

        /// <summary>
        /// Executes the login by starting a new session and loading the home window.
        /// </summary>
        private void Login()
        {
            //Starts a new session.
            App.StartSession(Username.Text, Password.Password);

            //If the remember button is checked, remembers the username.
            if (Remember.IsChecked == true) Settings.Instance.SetSetting("username", Username.Text);
            else Settings.Instance.SetSetting("username", null);

            //Loads the home window.
            new HomeWindow().Show();
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
