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
    /// Finestra principale.
    /// </summary>
    public partial class Home : Window, IDialogListener
    {
        private bool loaded = false;


        public Home()
        {
            InitializeComponent();
        }

        private void NewElement_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow(this, null);
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

        private void ElementPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow(this, ElementsManager.Instance.GetElementByID(((ItemElementPreview)sender).ID));
            passwordWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            LoadElements();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search.Text != "") LoadSearchedElements();
            else LoadElements();
        }

        public void OnDialogAbort() { }

        public void OnDialogAction(DialogAction action)
        {
            if (action != DialogAction.CANCEL)
            {
                if (Search.Text != "") LoadSearchedElements();
                else LoadElements();
            }
        }

        private void LoadSearchedElements() => LoadElements(ElementsManager.Instance.GetElementsByTitle(Search.Text));

        private void LoadElements(List<Element> elements) => ListElements.ItemsSource = elements;

        private void LoadElements() => ListElements.ItemsSource = ElementsManager.Instance.GetAll();

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
    }
}
