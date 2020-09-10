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
        public ElementPasswordWindow()
        {
            InitializeComponent();
        }


        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Save()
        {
            throw new NotImplementedException();
        }
    }
}
