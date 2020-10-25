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
            LoadElements();
        }

        private void ElementPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedElementPreview = (ItemElementPreview)sender;
            PasswordWindow passwordWindow = new PasswordWindow();
            passwordWindow.SetReceiver(this);
            passwordWindow.SetElement(ElementsManager.Instance.GetElementByID(selectedElementPreview.ID));
            passwordWindow.ShowDialog();
        }

        #region Switcher

        private void SwitchToPasswordSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded)
            {
                SwitchToCardSection.IsActivated = false;
                SwitchToNoteSection.IsActivated = false;
                CardSection.Visibility = Visibility.Collapsed;
                NoteSection.Visibility = Visibility.Collapsed;
                PasswordSection.Visibility = Visibility.Visible;
            }
        }

        private void SwitchToCardSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded)
            {
                SwitchToPasswordSection.IsActivated = false;
                SwitchToNoteSection.IsActivated = false;
                PasswordSection.Visibility = Visibility.Collapsed;
                NoteSection.Visibility = Visibility.Collapsed;
                CardSection.Visibility = Visibility.Visible;
            }
        }

        private void SwitchToNoteSection_ActivationChanged(object sender, SwitcherActivationChangedEventArgs e)
        {
            if (loaded)
            {
                SwitchToCardSection.IsActivated = false;
                SwitchToPasswordSection.IsActivated = false;
                CardSection.Visibility = Visibility.Collapsed;
                PasswordSection.Visibility = Visibility.Collapsed;
                NoteSection.Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            LoadElements();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search.Text != "")
            {
                LoadElements(ElementsManager.Instance.GetElementsByTitle(Search.Text));
            }
            else LoadElements();
        }

        private void LoadElements(List<Element> elements)
        {
            ListElements.ItemsSource = elements;
        }

        private void LoadElements()
        {
            ListElements.ItemsSource = ElementsManager.Instance.GetAll();
        }
    }
}
