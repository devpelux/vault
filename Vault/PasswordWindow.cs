using System.Windows;
using System.Windows.Input;

namespace Vault
{
    /// <summary>
    /// Finestra per la modifica delle password memorizzate.
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private readonly IDialogListener listener = null;
        Element element = null;


        public PasswordWindow(IDialogListener listener, Element element)
        {
            InitializeComponent();
            this.listener = listener;
            this.element = element;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (element != null)
            {
                Label.Text = element.Title;
                Website.Text = element.Website;
                Password.SetPassword(element.Password);
                Username.Text = element.Username;
                Details.Text = element.Details;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (element == null) AddElement();
            else EditElement();
            listener?.OnDialogAction(DialogAction.OK);
            Close();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            listener?.OnDialogAbort();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            listener?.OnDialogAction(DialogAction.CANCEL);
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
            element.Password = Password.GetPassword();
            element.Username = Username.Text;
            element.Details = Details.Text;
            element = ElementsManager.Instance.SaveElement(element);
            listener?.OnDialogAction(DialogAction.OK);
        }

        private void EditElement()
        {
            element.Category = "Password";
            element.Title = Label.Text;
            element.Website = Website.Text;
            element.Password = Password.GetPassword();
            element.Username = Username.Text;
            element.Details = Details.Text;
            element = ElementsManager.Instance.SaveElement(element);
            listener?.OnDialogAction(DialogAction.EDIT);
        }
    }
}
