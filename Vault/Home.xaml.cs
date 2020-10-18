using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using CustomControls;

namespace Vault
{
    /// <summary>
    /// Logica di interazione per Home.xaml
    /// </summary>
    public partial class Home : Window, IMessageReceiver
    {
        private ItemElementPreview selectedElementPreview = null;
        private bool loaded = false;


        public Home()
        {
            InitializeComponent();
        }

        private void NewElement_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
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

        public void ReceiveMessage(string message, object obj)
        {
            if (message == "added_password") _ = Container.Add(CreatePreview((Element)obj));
            else if (message == "edited_password") EditPreview(selectedElementPreview, (Element)obj);
            //else if (message == "delete") Container.Remove(selectedElementPreview);
            selectedElementPreview = null;
        }

        private ItemElementPreview CreatePreview(Element e)
        {
            ItemElementPreview ep = new ItemElementPreview();
            ep.MouseLeftButtonDown += ElementPreviewMouseLeftButtonDown;
            ep.ElementID = e.ID;
            ep.Title = e.Title;
            ep.Category = e.Category;
            ep.Details = e.Details;
            return ep;
        }

        private void EditPreview(ItemElementPreview preview, Element e)
        {
            preview.ElementID = e.ID;
            preview.Title = e.Title;
            preview.Category = e.Category;
            preview.Details = e.Details;
        }

        private void ElementPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedElementPreview = (ItemElementPreview)sender;
            PasswordWindow passwordWindow = new PasswordWindow();
            passwordWindow.SetReceiver(this);
            passwordWindow.SetElement(ElementsManager.Instance.GetElementByID(selectedElementPreview.ElementID));
            passwordWindow.ShowDialog();
        }

        private void SwitchToPasswordSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (!loaded || !e.IsActivated) return;
            SwitchToCardSection.IsActivated = false;
            SwitchToNoteSection.IsActivated = false;
            CardSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Collapsed;
            PasswordSection.Visibility = Visibility.Visible;
        }

        private void SwitchToCardSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (!loaded || !e.IsActivated) return;
            SwitchToPasswordSection.IsActivated = false;
            SwitchToNoteSection.IsActivated = false;
            PasswordSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Collapsed;
            CardSection.Visibility = Visibility.Visible;
        }

        private void SwitchToNoteSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (!loaded || !e.IsActivated) return;
            SwitchToCardSection.IsActivated = false;
            SwitchToPasswordSection.IsActivated = false;
            CardSection.Visibility = Visibility.Collapsed;
            PasswordSection.Visibility = Visibility.Collapsed;
            NoteSection.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
        }
    }
}
