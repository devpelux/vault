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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vault
{
    /// <summary>
    /// Logica di interazione per Home.xaml
    /// </summary>
    public partial class Home : Window, IMessageReceiver
    {
        public Home()
        {
            InitializeComponent();
        }

        private void NewElement_Click(object sender, RoutedEventArgs e)
        {
            ElementPasswordWindow passwordWindow = new ElementPasswordWindow();
            passwordWindow.SetReceiver(this);
            passwordWindow.ShowDialog();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Container.Clear();
            for (int i = 0; i < ElementsManager.Instance.Count; i++)
            {
                _ = Container.Add(CreatePreview(ElementsManager.Instance[i]));
            }
        }
        
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ToolbarMouseHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void SendMessage(string message, object obj)
        {
            if (message == "ok")
            {
                _ = Container.Add(CreatePreview((Element)obj));
            }
        }

        private ElementPreview CreatePreview(Element e)
        {
            ElementPreview ep = new ElementPreview();
            ep.Title = e.Title;
            ep.Category = e.Category;
            ep.Details = e.Details;
            return ep;
        }
    }
}
