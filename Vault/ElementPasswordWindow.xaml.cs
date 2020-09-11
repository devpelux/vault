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
    /// Logica di interazione per ElementPasswordWindow.xaml
    /// </summary>
    public partial class ElementPasswordWindow : Window
    {
        private IMessageReceiver receiver = null;
        Element element = null;


        public ElementPasswordWindow()
        {
            InitializeComponent();
        }


        public void SetReceiver(IMessageReceiver receiver) => this.receiver = receiver;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Save();
            receiver.SendMessage("ok", element);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            receiver.SendMessage("cancel", null);
            Close();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            receiver.SendMessage("close", null);
            Close();
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void Save()
        {
            element = new Element();
            element.Category = "Password";
            element.Title = Label.Text;
            element.Website = Website.Text;
            element.Password = Password.Text;
            element.Username = Username.Text;
            element.Details = Details.Text;
            ElementsManager.Instance.AddElement(element);
        }
    }
}
