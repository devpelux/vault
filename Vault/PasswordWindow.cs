using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vault
{
    /// <summary>
    /// Logica di interazione per PasswordWindow.xaml
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
            else Delete.Visibility = Visibility.Hidden;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (element == null) AddElement();
            else EditElement();
            Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteElement();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            receiver.ReceiveMessage("cancel", null);
            Close();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            receiver.ReceiveMessage("close", null);
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
            receiver.ReceiveMessage("add", element);
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
            receiver.ReceiveMessage("edit", element);
        }

        private void DeleteElement()
        {
            _ = ElementsManager.Instance.RemoveElement(element);
            receiver.ReceiveMessage("delete", element);
        }
    }
}
