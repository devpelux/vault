using FullControls.SystemComponents;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Settings;
using WpfCoreTools;

namespace Vault
{
    /// <summary>
    /// Window for input the credentials.
    /// </summary>
    public partial class CredentialsWindow : AvalonWindow, IDialog
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
                    Title = "Login";
                    break;
                case Request.Registration:
                    ConfirmPassword.Visibility = Visibility.Visible;
                    Username.Visibility = Visibility.Visible;
                    Remember.Visibility = Visibility.Visible;
                    RegisterLink.Visibility = Visibility.Collapsed;
                    LoginLink.Visibility = Visibility.Visible;
                    Title = "Registrazione";
                    break;
                case Request.Reauthentication:
                    ConfirmPassword.Visibility = Visibility.Collapsed;
                    Username.Visibility = Visibility.Collapsed;
                    Remember.Visibility = Visibility.Collapsed;
                    RegisterLink.Visibility = Visibility.Collapsed;
                    LoginLink.Visibility = Visibility.Collapsed;
                    Title = "Password";
                    break;
            }
        }

        /// <inheritdoc/>
        public object? GetResult() => ReauthResult;

        /// <summary>
        /// Executed when the window is loaded.
        /// Loads the username if is saved, then setups the window.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                    Login();
                    break;
                case Request.Registration:
                    Register();
                    break;
                case Request.Reauthentication:
                    Reauthenticate();
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
                new MessageWindow("Input non validi!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
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
                    new MessageWindow("Username o password errati o file di dati corrotto!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
                }
                return;
            }

            new MessageWindow("Username o password errati!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
        }

        /// <summary>
        /// Executes the registration.
        /// </summary>
        private void Register()
        {
            if (Username.TextLength == 0 || Password.PasswordLength == 0 || ConfirmPassword.PasswordLength == 0 || Username.Text.Contains(' '))
            {
                new MessageWindow("Input non validi!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
                return;
            }

            if (!Utility.ComparePasswords(Password.SecurePassword, ConfirmPassword.SecurePassword))
            {
                new MessageWindow("Le password non corrispondono!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
                return;
            }

            if (File.Exists(App.GetDBPath(Username.Text)))
            {
                new MessageWindow("Utente già esistente!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
                return;
            }

            DB.Context = new(App.GetDBPath(Username.Text), Password.Password);

            //Try to load the database instance to initialize it and check if the connection is ok.
            //If the connection fails aborts the launch and displays an error message.
            if (DB.Instance.IsConnected)
            {
                StartSession();
            }
            else
            {
                new MessageWindow("Impossibile creare il file di dati!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
            }
            return;
        }

        /// <summary>
        /// Executes the registration.
        /// </summary>
        private void Reauthenticate()
        {
            if (Password.PasswordLength == 0)
            {
                new MessageWindow("Input non validi!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
                return;
            }

            //Checks the password with the current session password.
            string? sessionPassword = InstanceSettings.Instance.GetSetting<string>("password");
            ReauthResult = Password.Password.Equals(sessionPassword);

            //Closes the window if the password is verified, otherwise displays an error message.
            if (ReauthResult) Close();
            else new MessageWindow("Password errata!", "Errore", MessageBoxImage.Exclamation).ShowDialog();
        }

        /// <summary>
        /// Starts the new session.
        /// </summary>
        private void StartSession()
        {
            //Save the username and password in the session settings.
            InstanceSettings.Instance.SetSetting("username", Username.Text);
            InstanceSettings.Instance.SetSetting("password", Password.Password);

            //If the remember button is checked, remembers the username.
            if (Remember.IsChecked == true) Settings.Instance.SetSetting("username", Username.Text);
            else Settings.Instance.SetSetting("username", null);

            //Loads the home window.
            new Home().Show();
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
