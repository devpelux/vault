using System.Windows;
using System.Windows.Input;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle password memorizzate.
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private IMessageReceiver receiver = null;
        Element element = null;


        public PasswordWindow()
        {
            InitializeComponent();
        }


        public void SetReceiver(IMessageReceiver receiver) => this.receiver = receiver;

        public void SetElement(Element element) => this.element = element;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (element != null)
            {
                Label.Text = element.Title;
                Website.Text = element.Website;
                Password.Text = element.Password;
                Username.Text = element.Username;
                Details.Text = element.Details;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (element == null) AddElement();
            else EditElement();
            Close();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void AddElement()
        {
            element = new Element();
            element.Category = "Password";
            element.Title = Label.Text;
            element.Website = Website.Text;
            element.Password = Password.Text;
            element.Username = Username.Text;
            element.Details = Details.Text;
            element = ElementsManager.Instance.SaveElement(element);
            receiver.ReceiveMessage("added_password", element);
        }

        private void EditElement()
        {
            element.Category = "Password";
            element.Title = Label.Text;
            element.Website = Website.Text;
            element.Password = Password.Text;
            element.Username = Username.Text;
            element.Details = Details.Text;
            element = ElementsManager.Instance.SaveElement(element);
            receiver.ReceiveMessage("edited_password", element);
        }
    }
}
